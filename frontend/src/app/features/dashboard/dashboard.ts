import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Menu } from '../../shared/menu/menu';
import { Carregamento } from '../../shared/carregamento/carregamento';
import { AutenticacaoService } from '../../core/services/autenticacao.service';
import { RefeicaoService } from '../../core/services/refeicao.service';
import { FormsModule } from '@angular/forms';
import { GraficoService } from '../../core/services/grafico.service';
import { AdicionarRefeicao } from '../../shared/adicionar-refeicao/adicionar-refeicao';
import { AdicionarExercicio } from "../../shared/adicionar-exercicio/adicionar-exercicio";
import { GraficoDiario } from "../../shared/grafico-diario/grafico-diario";
import { AtividadeFisicaService } from '../../core/services/atividade-fisica.service';
import Swal from 'sweetalert2';
import { NotificacaoService } from '../../core/services/notificacao.service';
import { BsGraficoHistoricoMensalComponent, DadoHistorico } from '../../shared/bs-grafico-historico-mensal/bs-grafico-historico-mensal';
import { BsGraficoMediaSemanalComponent } from '../../shared/bs-grafico-media-semanal/bs-grafico-media-semanal';
import { BsCardConsistenciaComponent } from '../../shared/bs-card-consistencia/bs-card-consistencia';
import { BsCardEquilibrioEnergeticoComponent } from '../../shared/bs-card-equilibrio-energetico/bs-card-equilibrio-energetico';
import { BsCardImpactoEstimadoComponent } from '../../shared/bs-card-impacto-estimado/bs-card-impacto-estimado';

// Novos componentes
import { ConsumoCaloricoComponent } from '../relatorios/consumo-calorico/consumo-calorico';
import { GastoCaloricoComponent } from '../relatorios/gasto-calorico/gasto-calorico';
import { NutrientesComponent } from '../relatorios/nutrientes/nutrientes';
import { PerfilComponent } from '../perfil/perfil/perfil';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule, Carregamento, FormsModule, AdicionarRefeicao, AdicionarExercicio, 
    GraficoDiario, Menu, 
    BsGraficoHistoricoMensalComponent, BsGraficoMediaSemanalComponent, 
    BsCardConsistenciaComponent, BsCardEquilibrioEnergeticoComponent, 
    BsCardImpactoEstimadoComponent,
    ConsumoCaloricoComponent, GastoCaloricoComponent, NutrientesComponent, PerfilComponent
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {
  refeicaoService = inject(RefeicaoService);
  atividadeFisicaService = inject(AtividadeFisicaService);
  graficoService = inject(GraficoService);
  autenticacao = inject(AutenticacaoService);
  notificacaoService = inject(NotificacaoService);

  todayDate = new Date().toLocaleDateString('en-CA', { timeZone: 'America/Sao_Paulo' });
  dataSelecionada = signal<string>(this.todayDate);

  abaMenu = signal<'dashboard' | 'estatisticas-consumo' | 'estatisticas-gasto' | 'estatisticas-nutrientes' | 'perfil'>('dashboard');
  menuAberto = signal<boolean>(false);
  graficoDashboard = signal<'diario' | 'semanal' | 'mensal'>('diario');

  dadosHistoricoSaldoMensal = computed<DadoHistorico[]>(() => {
    return this.dadosGraficoMensal().map(p => ({ legenda: p.legenda, valor: p.saldoCalorico }));
  });

  dadosGraficoSemanalSaldo = computed<DadoHistorico[]>(() => {
    return this.dadosGraficoSemanal().map(p => ({ legenda: p.legenda, valor: p.saldoCalorico }));
  });

  dashboardInsightsSemanal = signal<any>(null);
  dashboardInsightsMensal = signal<any>(null);

  metaCalorias = signal<number>(0);
  caloriasConsumidas = signal<number>(0);
  caloriasQueimadas = signal<number>(0);
  caloriasCalculadas = signal<number>(0);
  refeicoesDeHoje = signal<any[]>([]);
  exerciciosDeHoje = signal<any[]>([]);
  dadosGraficoSemanal = signal<any[]>([]);
  dadosGraficoMensal = signal<any[]>([]);

  mapaRefeicoes: Record<number, any> = this.refeicaoService.obterMapaRefeicoes(); 
  mapaExercicios: Record<number, any> = this.atividadeFisicaService.obterMapaExercicios();

  ngOnInit(): void {
    this.obterGraficoDiario();
    this.notificacaoService.iniciarConexao();
    this.notificacaoService.refeicaoProcessada$.subscribe(() => {
      this.atualizarDadosAbaAtiva();
      Swal.mixin({ toast: true, position: 'top-end', showConfirmButton: false, timer: 3000, timerProgressBar: true })
          .fire({ icon: 'success', title: 'Refeição processada!', text: 'Os dados foram atualizados automaticamente.' });
    });
  }

  diaAnterior() {
    const data = new Date(this.dataSelecionada());
    data.setDate(data.getDate() - 1);
    this.dataSelecionada.set(data.toISOString().split('T')[0]);
    this.atualizarDadosAbaAtiva();
  }

  diaSeguinte() {
    if (this.dataSelecionada() === this.todayDate) return;
    const data = new Date(this.dataSelecionada());
    data.setDate(data.getDate() + 1);
    this.dataSelecionada.set(data.toISOString().split('T')[0]);
    this.atualizarDadosAbaAtiva();
  }

  onDataAlterada() { this.atualizarDadosAbaAtiva(); }

  private atualizarDadosAbaAtiva() {
    if (this.abaMenu() === 'dashboard') {
      if (this.graficoDashboard() === 'diario') this.obterGraficoDiario();
      else if (this.graficoDashboard() === 'semanal') this.obterGraficoSemanal();
      else if (this.graficoDashboard() === 'mensal') this.obterGraficoMensal();
    }
  }

  formatarDataExibicao(dataIso: string): string {
    if (dataIso === this.todayDate) return 'Hoje';
    const partes = dataIso.split('-');
    const data = new Date(Number(partes[0]), Number(partes[1]) - 1, Number(partes[2]));
    return data.toLocaleDateString('pt-BR', { day: '2-digit', month: 'short', year: 'numeric' });
  }

  mostrarModalRefeicao = signal<boolean>(false);
  mostrarModalExercicio = signal<boolean>(false);

  alterarAbaMenu(aba: any) {
    this.abaMenu.set(aba);
    this.menuAberto.set(false);
    this.atualizarDadosAbaAtiva();
  }

  alterarAbaGrafico(aba: 'diario' | 'semanal' | 'mensal') {
    this.graficoDashboard.set(aba);
    this.atualizarDadosAbaAtiva();
  }

  obterGraficoDiario() {
    this.graficoService.obterGraficoDiario(this.autenticacao.obterId(), this.dataSelecionada()).subscribe((res: any) => {
      this.metaCalorias.set(res.metaCaloricaDiaria);
      this.caloriasConsumidas.set(res.totalCaloriasConsumidas);
      this.caloriasQueimadas.set(res.totalCaloriasGastas);
      this.caloriasCalculadas.set(res.caloriasCalculadas);
      this.refeicoesDeHoje.set(res.refeicoes);
      this.exerciciosDeHoje.set(res.exercicios);
    });
  }

  obterGraficoSemanal() {
    this.graficoService.obterGraficoSemanal(this.autenticacao.obterId(), this.dataSelecionada()).subscribe((res: any) => {
      this.dadosGraficoSemanal.set(res.pontos);
      this.dashboardInsightsSemanal.set(res.insights);
    });
  }

  obterGraficoMensal() {
    this.graficoService.obterGraficoMensal(this.autenticacao.obterId(), this.dataSelecionada()).subscribe((res: any) => {
      this.dadosGraficoMensal.set(res.pontos);
      this.dashboardInsightsMensal.set(res.insights);
    });
  }

  excluirRefeicao(id: number) {
    Swal.fire({ title: 'Excluir refeição?', text: "Esta ação não pode ser desfeita.", icon: 'warning', showCancelButton: true, confirmButtonColor: '#10b981', cancelButtonColor: '#ef4444', confirmButtonText: 'Sim, excluir!' }).then((result) => {
      if (result.isConfirmed) {
        this.refeicaoService.excluir(id).subscribe(() => {
          Swal.fire('Excluído!', 'Sua refeição foi removida.', 'success');
          this.obterGraficoDiario();
        });
      }
    });
  }

  excluirExercicio(id: number) {
    Swal.fire({ title: 'Excluir exercício?', text: "Esta ação não pode ser desfeita.", icon: 'warning', showCancelButton: true, confirmButtonColor: '#10b981', cancelButtonColor: '#ef4444', confirmButtonText: 'Sim, excluir!' }).then((result) => {
      if (result.isConfirmed) {
        this.atividadeFisicaService.excluir(id).subscribe(() => {
          Swal.fire('Excluído!', 'Seu exercício foi removido.', 'success');
          this.obterGraficoDiario();
        });
      }
    });
  }
}

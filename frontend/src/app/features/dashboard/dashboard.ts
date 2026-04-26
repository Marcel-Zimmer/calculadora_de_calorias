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
import { PerfilBiometricoService } from '../../core/services/perfil-biometrico.service';
import { RegistroFisicoService } from '../../core/services/registro-fisico.service';
import { UsuarioService } from '../../core/services/usuario.service';
import { EstatisticasNutrientesComponent } from '../estatisticas-nutrientes/estatisticas-nutrientes';
import { NotificacaoService } from '../../core/services/notificacao.service';
import { BsGraficoHistoricoMensalComponent, DadoHistorico } from '../../shared/bs-grafico-historico-mensal/bs-grafico-historico-mensal';
import { BsGraficoMediaSemanalComponent } from '../../shared/bs-grafico-media-semanal/bs-grafico-media-semanal';
import { BsCardConsistenciaComponent } from '../../shared/bs-card-consistencia/bs-card-consistencia';
import { BsCardEquilibrioEnergeticoComponent } from '../../shared/bs-card-equilibrio-energetico/bs-card-equilibrio-energetico';
import { BsCardImpactoEstimadoComponent } from '../../shared/bs-card-impacto-estimado/bs-card-impacto-estimado';
import { BsCardMediaItemComponent } from '../../shared/bs-card-media-item/bs-card-media-item';
import { BsGraficoDistribuicaoItensComponent } from '../../shared/bs-grafico-distribuicao-itens/bs-grafico-distribuicao-itens';
import { BsCardMaiorIngestaoComponent } from '../../shared/bs-card-maior-ingestao/bs-card-maior-ingestao';
import { BsGraficoMediaPorSemanaComponent } from '../../shared/bs-grafico-media-por-semana/bs-grafico-media-por-semana';
import { BsGraficoMaioresConsumosComponent } from '../../shared/bs-grafico-maiores-consumos/bs-grafico-maiores-consumos';
import { BsCardMaiorGastoComponent } from '../../shared/bs-card-maior-gasto/bs-card-maior-gasto';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule, Carregamento, FormsModule, AdicionarRefeicao, AdicionarExercicio, 
    GraficoDiario, Menu, EstatisticasNutrientesComponent, 
    BsGraficoHistoricoMensalComponent, BsGraficoMediaSemanalComponent, 
    BsCardConsistenciaComponent, BsCardEquilibrioEnergeticoComponent, 
    BsCardImpactoEstimadoComponent, BsCardMediaItemComponent, 
    BsGraficoDistribuicaoItensComponent, BsCardMaiorIngestaoComponent, 
    BsGraficoMediaPorSemanaComponent, BsGraficoMaioresConsumosComponent,
    BsCardMaiorGastoComponent
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {
  refeicaoService = inject(RefeicaoService);
  atividadeFisicaService = inject(AtividadeFisicaService);
  graficoService = inject(GraficoService);
  perfilService = inject(PerfilBiometricoService);
  registroFisicoService = inject(RegistroFisicoService);
  usuarioService = inject(UsuarioService);
  autenticacao = inject(AutenticacaoService);
  notificacaoService = inject(NotificacaoService);

  Math = Math;

  todayDate = new Date().toLocaleDateString('en-CA', { timeZone: 'America/Sao_Paulo' });
  dataSelecionada = signal<string>(this.todayDate);

  abaMenu = signal<'dashboard' | 'estatisticas-consumo' | 'estatisticas-gasto' | 'estatisticas-nutrientes' | 'perfil'>('dashboard');
  menuAberto = signal<boolean>(false);
  graficoDashboard = signal<'diario' | 'semanal' | 'mensal'>('diario');
  subAbaPerfil = signal<'biometrico' | 'fisico' | 'seguranca'>('biometrico');
  
  periodoEstatisticas = signal<'semanal' | 'mensal'>('semanal');
  dadosEstatisticas = signal<any>(null);

  dadosHistoricoSaldoMensal = computed<DadoHistorico[]>(() => {
    const pontos = this.dadosGraficoMensal();
    return pontos.map(p => ({ legenda: p.legenda, valor: p.saldoCalorico }));
  });

  dadosGraficoSemanalSaldo = computed<DadoHistorico[]>(() => {
    const pontos = this.dadosGraficoSemanal();
    return pontos.map(p => ({ legenda: p.legenda, valor: p.saldoCalorico }));
  });

  dadosHistoricoConsumo = computed<DadoHistorico[]>(() => {
    const dados = this.dadosEstatisticas();
    if (!dados) return [];
    return dados.pontos.map((p: any) => ({ legenda: p.legenda, valor: p.caloriasConsumidas }));
  });

  dadosHistoricoGasto = computed<DadoHistorico[]>(() => {
    const dados = this.dadosEstatisticas();
    if (!dados) return [];
    return dados.pontos.map((p: any) => ({ legenda: p.legenda, valor: p.caloriasGastas }));
  });

  cardsMediaConsumo = computed(() => {
    const dados = this.dadosEstatisticas();
    if (!dados) return [];
    const lista = [];
    lista.push({ icone: '📊', corCss: 'bg-slate-50 text-indigo-500', titulo: 'Média Diária', valor: dados.mediaConsumoDiario || 0, legenda: 'no período' });
    const refeicoesEnriquecidas = dados.distribuicaoRefeicoes || [];
    refeicoesEnriquecidas.forEach((refeicao: any) => {
        lista.push({ icone: refeicao.icone, corCss: refeicao.corCss, titulo: refeicao.displayNome, valor: refeicao.media || 0, legenda: 'média diária' });
    });
    return lista;
  });

  dashboardInsightsSemanal = signal<any>(null);
  dashboardInsightsMensal = signal<any>(null);

  insightsConsumo = computed(() => {
    const dados = this.dadosEstatisticas();
    if (!dados || !dados.pontos || dados.pontos.length === 0) return null;
    const pontos = dados.pontos;
    const diasNaMeta = pontos.filter((p: any) => p.caloriasConsumidas > 0 && p.caloriasConsumidas <= dados.metaCaloricaDiaria).length;
    const maiorIngestao = Math.max(...pontos.map((p: any) => p.caloriasConsumidas));
    const diaMaiorIngestao = pontos.find((p: any) => p.caloriasConsumidas === maiorIngestao)?.legenda || '-';
    return { maiorIngestao, diaMaiorIngestao, diasNaMeta, totalDias: pontos.length };
  });

  insightsGasto = computed(() => {
    const dados = this.dadosEstatisticas();
    if (!dados || !dados.pontos || dados.pontos.length === 0) return null;
    const pontos = dados.pontos;
    const diasAtivos = pontos.filter((p: any) => p.caloriasGastas > 0).length;
    const maiorGasto = Math.max(...pontos.map((p: any) => p.caloriasGastas));
    const diaMaiorGasto = pontos.find((p: any) => p.caloriasGastas === maiorGasto)?.legenda || '-';
    const exercicioPrincipal = dados.gastoInsights?.exercicioPrincipal || 'Nenhum';
    return { diasAtivos, totalDias: pontos.length, maiorGasto, diaMaiorGasto, exercicioPrincipal };
  });

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
    } else if (this.abaMenu() === 'estatisticas-consumo' || this.abaMenu() === 'estatisticas-gasto') {
      this.carregarEstatisticas();
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
  imagemRefeicao = signal<string | null>(null);

  perfilId = signal<number>(0);
  dataNascimento = signal<string>('');
  genero = signal<number>(1);
  alturaCm = signal<number>(0);
  nivelAtividade = signal<number>(1);
  objetivo = signal<number>(1);

  registroFisicoId = signal<number>(0);
  pesoKg = signal<number>(0);
  metaCaloricaDiaria = signal<number>(0);

  novaSenha = signal<string>('');
  confirmarNovaSenha = signal<string>('');

  alterarAbaMenu(aba: 'dashboard' | 'estatisticas-consumo' | 'estatisticas-gasto' | 'estatisticas-nutrientes' | 'perfil') {
    this.abaMenu.set(aba);
    if (aba === 'perfil') this.carregarDadosPerfil();
    else if (aba === 'estatisticas-consumo' || aba === 'estatisticas-gasto') this.carregarEstatisticas();
  }

  alterarPeriodoEstatisticas(periodo: 'semanal' | 'mensal') {
    this.periodoEstatisticas.set(periodo);
    this.carregarEstatisticas();
  }

  carregarEstatisticas() {
    const userId = this.autenticacao.obterId();
    const obs = this.periodoEstatisticas() === 'semanal' 
      ? this.graficoService.obterEstatisticasSemanais(userId, this.dataSelecionada())
      : this.graficoService.obterEstatisticasMensais(userId, this.dataSelecionada());

    obs.subscribe({
      next: (res: any) => {
        res.distribuicaoExercicios = res.distribuicaoExercicios.map((item: any) => {
          const info = this.mapaExercicios[Number(item.nome)];
          return { ...item, displayNome: info?.nome || 'Outro', icone: info?.icone || '🔥', corCss: info?.cor || 'bg-slate-50 text-slate-500' };
        });
        res.distribuicaoRefeicoes = res.distribuicaoRefeicoes.map((item: any) => {
          const info = this.mapaRefeicoes[Number(item.nome)];
          return { ...item, displayNome: info?.nome || 'Outro', icone: info?.icone || '🍽️', corCss: info?.cor || 'bg-slate-50 text-slate-500' };
        });
        this.dadosEstatisticas.set(res);
      }
    });
  }

  alterarAbaGrafico(aba: 'diario' | 'semanal' | 'mensal') {
    this.graficoDashboard.set(aba);
    if (aba === 'semanal') this.obterGraficoSemanal();
    else if (aba === 'mensal') this.obterGraficoMensal();
    else this.obterGraficoDiario();
  }

  carregarDadosPerfil() {
    const userId = this.autenticacao.obterId();
    this.perfilService.obterPorUsuarioId(userId).subscribe((res: any) => {
      this.perfilId.set(res.id);
      this.dataNascimento.set(res.dataNascimento.split('T')[0]);
      this.genero.set(res.genero);
      this.alturaCm.set(res.alturaCm);
      this.nivelAtividade.set(res.nivelAtividade);
      this.objetivo.set(res.objetivo);
    });
    this.registroFisicoService.obterUltimoPorUsuarioId(userId).subscribe((res: any) => {
      this.registroFisicoId.set(res.id);
      this.pesoKg.set(res.pesoKg);
      this.metaCaloricaDiaria.set(res.metaCaloricaDiaria);
    });
  }

  salvarPerfilBiometrico() {
    const dados = { usuarioId: this.autenticacao.obterId(), dataNascimento: this.dataNascimento(), genero: Number(this.genero()), alturaCm: Number(this.alturaCm()), nivelAtividade: Number(this.nivelAtividade()), objetivo: Number(this.objetivo()) };
    this.perfilService.atualizar(dados.usuarioId, dados).subscribe(() => {
      Swal.fire({ title: 'Sucesso!', text: 'Perfil atualizado.', icon: 'success', confirmButtonColor: '#10b981' });
      this.obterGraficoDiario();
    });
  }

  salvarRegistroFisico() {
    const dados = { usuarioId: this.autenticacao.obterId(), pesoKg: Number(this.pesoKg()), metaCaloricaDiaria: Number(this.metaCaloricaDiaria()) };
    this.registroFisicoService.atualizar(dados.usuarioId, dados).subscribe(() => {
      Swal.fire({ title: 'Sucesso!', text: 'Medidas atualizadas.', icon: 'success', confirmButtonColor: '#10b981' });
      this.obterGraficoDiario();
    });
  }

  salvarNovaSenha() {
    if (!this.novaSenha() || this.novaSenha().length < 6) { Swal.fire({ title: 'Atenção', text: 'A senha deve ter no mínimo 6 caracteres.', icon: 'warning' }); return; }
    if (this.novaSenha() !== this.confirmarNovaSenha()) { Swal.fire({ title: 'Erro', text: 'As senhas não coincidem.', icon: 'error' }); return; }
    this.usuarioService.atualizarSenha(this.novaSenha()).subscribe(() => {
      Swal.fire({ title: 'Sucesso!', text: 'Senha atualizada com sucesso.', icon: 'success', confirmButtonColor: '#10b981' });
      this.novaSenha.set(''); this.confirmarNovaSenha.set('');
    });
  }

  obterGraficoDiario(){
    this.graficoService.obterGraficoDiario(this.autenticacao.obterId(), this.dataSelecionada()).subscribe((resposta: any) => {
      this.metaCalorias.set(resposta.metaCaloricaDiaria);
      this.caloriasConsumidas.set(resposta.totalCaloriasConsumidas);
      this.refeicoesDeHoje.set(resposta.refeicoes);
      this.caloriasQueimadas.set(resposta.totalCaloriasGastas);
      this.exerciciosDeHoje.set(resposta.exercicios);
      this.caloriasCalculadas.set(resposta.caloriasCalculadas);
    });
  }

  obterGraficoSemanal() {
    this.graficoService.obterGraficoSemanal(this.autenticacao.obterId(), this.dataSelecionada()).subscribe((resposta: any) => {
      this.dadosGraficoSemanal.set(resposta.pontos);
      this.metaCalorias.set(resposta.metaCaloricaDiaria);
      this.caloriasConsumidas.set(resposta.totalCaloriasConsumidas);
      this.caloriasQueimadas.set(resposta.totalCaloriasGastas);
      this.caloriasCalculadas.set(resposta.caloriasCalculadas);
      this.dashboardInsightsSemanal.set(resposta.insights);
    });
  }

  obterGraficoMensal() {
    this.graficoService.obterGraficoMensal(this.autenticacao.obterId(), this.dataSelecionada()).subscribe((resposta: any) => {
      this.dadosGraficoMensal.set(resposta.pontos);
      this.metaCalorias.set(resposta.metaCaloricaDiaria);
      this.caloriasConsumidas.set(resposta.totalCaloriasConsumidas);
      this.caloriasQueimadas.set(resposta.totalCaloriasGastas);
      this.caloriasCalculadas.set(resposta.caloriasCalculadas);
      this.dashboardInsightsMensal.set(resposta.insights);
    });
  }

  excluirRefeicao(id: number) {
    Swal.fire({ title: 'Excluir Refeição?', text: "Esta ação não pode ser revertida!", icon: 'warning', showCancelButton: true, confirmButtonColor: '#10b981', cancelButtonColor: '#f43f5e', confirmButtonText: 'Sim, excluir!', cancelButtonText: 'Cancelar' }).then((result) => {
      if (result.isConfirmed) {
        this.refeicaoService.excluir(id).subscribe(() => {
          this.obterGraficoDiario();
          Swal.fire({ title: 'Excluído!', text: 'Sua refeição foi removida.', icon: 'success', confirmButtonColor: '#10b981' });
        });
      }
    });
  }

  excluirExercicio(id: number) {
    Swal.fire({ title: 'Excluir Exercício?', text: "Esta ação não pode ser revertida!", icon: 'warning', showCancelButton: true, confirmButtonColor: '#10b981', cancelButtonColor: '#f43f5e', confirmButtonText: 'Sim, excluir!', cancelButtonText: 'Cancelar' }).then((result) => {
      if (result.isConfirmed) {
        this.atividadeFisicaService.excluir(id).subscribe(() => {
          this.obterGraficoDiario();
          Swal.fire({ title: 'Excluído!', text: 'Seu exercício foi removido.', icon: 'success', confirmButtonColor: '#10b981' });
        });
      }
    });
  }

  formatarTempo(tempo: string | null | undefined): string {
    if (!tempo) return 'Tempo não registrado';
    const partes = tempo.split(':');
    if (partes.length < 2) return tempo;
    const horas = parseInt(partes[0], 10);
    const minutos = parseInt(partes[1], 10);
    let texto = 'Tempo:';
    if (horas > 0) texto += ` ${horas} hora${horas > 1 ? 's' : ''}`;
    if (minutos > 0) { if (horas > 0) texto += ' e '; texto += `${minutos} min`; }
    return texto;
  }
}

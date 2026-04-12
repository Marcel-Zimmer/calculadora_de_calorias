import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Menu } from '../../shared/menu/menu';
import { Ui } from '../../core/services/ui.service';
import { Carregamento } from '../../shared/carregamento/carregamento';
import { AutenticacaoService } from '../../core/services/autenticacao.service';
import { RefeicaoService } from '../../core/services/refeicao.service';
import { FormsModule } from '@angular/forms';
import { GraficoService } from '../../core/services/grafico.service';
import { AdicionarRefeicao } from '../../shared/adicionar-refeicao/adicionar-refeicao';
import { AdicionarExercicio } from "../../shared/adicionar-exercicio/adicionar-exercicio";
import { GraficoDiario } from "../../shared/grafico-diario/grafico-diario";
import { GraficoSemanal } from "../../shared/grafico-semanal/grafico-semanal";
import { GraficoMensal } from "../../shared/grafico-mensal/grafico-mensal";


import { AtividadeFisicaService } from '../../core/services/atividade-fisica.service';


import Swal from 'sweetalert2';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, Carregamento, FormsModule, AdicionarRefeicao, AdicionarExercicio, GraficoDiario, GraficoSemanal, GraficoMensal],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})

export class Dashboard implements OnInit {
  refeicaoService = inject(RefeicaoService);
  atividadeFisicaService = inject(AtividadeFisicaService);
  graficoService = inject(GraficoService)
  ngOnInit(): void {
    this.obterGraficoDiario();
  }
  autenticacao = inject(AutenticacaoService);
  abaMenu = signal<'dashboard' | 'estatisticas' | 'perfil'>('dashboard');
  menuAberto = signal<boolean>(false);
  graficoDashboard = signal<'diario' | 'semanal' | 'mensal'>('diario');

  mostrarModalRefeicao = signal<boolean>(false);
  mostrarModalExercicio = signal<boolean>(false);
  imagemRefeicao = signal<string | null>(null);

  todayDate = new Date().toISOString().split('T')[0];

  metaCalorias = signal<number>(0);
  caloriasConsumidas = signal<number>(0);
  caloriasQueimadas = signal<number>(0);
  refeicoesDeHoje = signal<any[]>([]);
  exerciciosDeHoje = signal<any[]>([]);

  dadosGraficoSemanal = signal<any[]>([]);
  dadosGraficoMensal = signal<any[]>([]);

  alterarAbaGrafico(aba: 'diario' | 'semanal' | 'mensal') {
    this.graficoDashboard.set(aba);
    if (aba === 'semanal') {
      this.obterGraficoSemanal();
    } else if (aba === 'mensal') {
      this.obterGraficoMensal();
    } else {
      this.obterGraficoDiario();
    }
  }

  obterGraficoSemanal() {
    this.graficoService.obterGraficoSemanal(this.autenticacao.obterId())
      .subscribe({
        next: (resposta: any) => {
          this.dadosGraficoSemanal.set(resposta.pontos);
          this.metaCalorias.set(resposta.metaCaloricaDiaria);
        },
        error: (erro) => console.error('Falha ao obter gráfico semanal', erro)
      });
  }

  obterGraficoMensal() {
    this.graficoService.obterGraficoMensal(this.autenticacao.obterId())
      .subscribe({
        next: (resposta: any) => {
          this.dadosGraficoMensal.set(resposta.pontos);
          this.metaCalorias.set(resposta.metaCaloricaDiaria);
        },
        error: (erro) => console.error('Falha ao obter gráfico mensal', erro)
      });
  }

  mapaRefeicoes: Record<number, any> = {
    1: { nome: 'Café da Manhã', icone: '☕', cor: 'bg-orange-100 text-orange-500' },
    2: { nome: 'Almoço',        icone: '🍽️', cor: 'bg-emerald-100 text-emerald-500' },
    3: { nome: 'Jantar',        icone: '🌙', cor: 'bg-blue-100 text-blue-500' },
    4: { nome: 'Lanche',        icone: '🥪', cor: 'bg-purple-100 text-purple-500' }
  }; 

  mapaExercicios: Record<number, any> = {
      1: { nome: 'Ciclismo',   icone: '🚴', cor: 'bg-sky-100 text-sky-600' },
      2: { nome: 'Boxe',       icone: '🥊', cor: 'bg-red-100 text-red-600' },
      3: { nome: 'Musculação', icone: '🏋️', cor: 'bg-slate-100 text-slate-600' },
      4: { nome: 'Corrida',    icone: '🏃', cor: 'bg-amber-100 text-amber-600' },
      5: { nome: 'Natação',    icone: '🏊', cor: 'bg-cyan-100 text-cyan-600' }
  };

  averageConsumed = computed(() => this.graficoDashboard() === 'semanal' ? 0 : 0);
  averageBurned = computed(() => this.graficoDashboard() === 'semanal' ? 0 : 0);

  weeklyChartData = [
    { label: 'SEG', kcal: 1900 }, { label: 'TER', kcal: 2300 },
    { label: 'QUA', kcal: 1850 }, { label: 'QUI', kcal: 2150 },
    { label: 'SEX', kcal: 1950 }, { label: 'SÁB', kcal: 1450 },
    { label: 'DOM', kcal: 0 }
  ];

  monthlyChartData = Array.from({ length: 30 }, (_, i) => ({
    label: (i + 1).toString(),
    kcal: i > 23 ? 0 : Math.floor(Math.random() * 800) + 1600
  }));

  closeMealModal() {
    this.mostrarModalRefeicao.set(false);
    this.imagemRefeicao.set(null);
  }

  tipoRefeicao = signal<number>(1);
  dataRefeicao = signal<string>(this.todayDate);
  pesoRefeicao = signal<number | null>(null);
  apelidoRefeicao = signal<string>('');

  fotoReal = signal<File | null>(null);

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.fotoReal.set(file);

      const reader = new FileReader();
      reader.onload = (e: any) => this.imagemRefeicao.set(e.target.result);
      reader.readAsDataURL(file);
    }
  }

  salvarRefeicao() {
    let novaRefeicao: FormData = new FormData;
    novaRefeicao.append("Apelido", this.apelidoRefeicao());
    novaRefeicao.append("Tipo", this.tipoRefeicao().toString());
    novaRefeicao.append("Data", this.dataRefeicao());
    novaRefeicao.append("PesoEmGramas", this.pesoRefeicao()?.toString() ?? "");
    novaRefeicao.append("UsuarioId", this.autenticacao.obterId().toString());
    const foto = this.fotoReal();
    if (foto) {
      novaRefeicao.append("Imagem", foto);
    }

    this.refeicaoService.adicionar(novaRefeicao)
      .subscribe({
        next: (resposta: any) => {
          console.log('Cadastrado com sucesso!', resposta);
          this.obterGraficoDiario();
        },
        error: (erro) => {
          console.error('Falha ', erro);
        }
      });
    //this.limparEFecharModal();
  }

  obterGraficoDiario(){
    this.graficoService.obterGraficoDiario(this.autenticacao.obterId())
      .subscribe({
        next: (resposta: any) => {
          console.log('grafico obtido com sucesso', resposta);
          this.receberGraficoDiario(resposta);
        },
        error: (erro) => {
          console.error('Falha ', erro);
        }
      });
  }
  receberGraficoDiario(resposta:any){
      this.metaCalorias.set(resposta.metaCaloricaDiaria);
      this.caloriasConsumidas.set(resposta.totalCaloriasConsumidas);
      this.refeicoesDeHoje.set(resposta.refeicoes)
      this.caloriasQueimadas.set(resposta.totalCaloriasGastas);
      this.exerciciosDeHoje.set(resposta.exercicios)
  }

  limparEFecharModal() {
    this.tipoRefeicao.set(1);
    this.dataRefeicao.set(this.todayDate);
    this.pesoRefeicao.set(null);
    this.apelidoRefeicao.set('');
    this.fotoReal.set(null);
    this.closeMealModal();
  }

  formatarTempo(tempo: string | null | undefined): string {
    if (!tempo) return 'Tempo não registrado';

    const partes = tempo.split(':');
    if (partes.length < 2) return tempo;

    const horas = parseInt(partes[0], 10);
    const minutos = parseInt(partes[1], 10);

    let texto = 'Tempo:';

    if (horas > 0) {
      texto += `${horas} hora${horas > 1 ? 's' : ''}`;
    }

    if (minutos > 0) {
      if (horas > 0) texto += ' e ';
      texto += `${minutos} min`;
    }

    if (horas === 0 && minutos === 0) {
      return 'Tempo: Menos de 1 min';
    }

    return texto;
  }  

  excluirRefeicao(id: number) {
    Swal.fire({
      title: 'Excluir Refeição?',
      text: "Esta ação não pode ser revertida!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#10b981',
      cancelButtonColor: '#f43f5e',
      confirmButtonText: 'Sim, excluir!',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.refeicaoService.excluir(id).subscribe({
          next: () => {
            this.obterGraficoDiario();
            Swal.fire({
              title: 'Excluído!',
              text: 'Sua refeição foi removida.',
              icon: 'success',
              confirmButtonColor: '#10b981'
            });
          },
          error: (erro) => {
            console.error('Falha ao excluir refeição', erro);
            Swal.fire({
              title: 'Erro!',
              text: 'Não foi possível excluir a refeição.',
              icon: 'error',
              confirmButtonColor: '#10b981'
            });
          }
        });
      }
    });
  }

  excluirExercicio(id: number) {
    Swal.fire({
      title: 'Excluir Exercício?',
      text: "Esta ação não pode ser revertida!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#10b981',
      cancelButtonColor: '#f43f5e',
      confirmButtonText: 'Sim, excluir!',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.atividadeFisicaService.excluir(id).subscribe({
          next: () => {
            this.obterGraficoDiario();
            Swal.fire({
              title: 'Excluído!',
              text: 'Seu exercício foi removido.',
              icon: 'success',
              confirmButtonColor: '#10b981'
            });
          },
          error: (erro) => {
            console.error('Falha ao excluir exercício', erro);
            Swal.fire({
              title: 'Erro!',
              text: 'Não foi possível excluir o exercício.',
              icon: 'error',
              confirmButtonColor: '#10b981'
            });
          }
        });
      }
    });
  }
  }
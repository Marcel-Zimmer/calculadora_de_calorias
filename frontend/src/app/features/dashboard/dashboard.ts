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
import { PerfilBiometricoService } from '../../core/services/perfil-biometrico.service';
import { RegistroFisicoService } from '../../core/services/registro-fisico.service';
import { environment } from '../../../environments/environment';
import { UsuarioService } from '../../core/services/usuario.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, Carregamento, FormsModule, AdicionarRefeicao, AdicionarExercicio, GraficoDiario, GraficoSemanal, GraficoMensal, Menu],
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

  // Navegação
  abaMenu = signal<'dashboard' | 'estatisticas' | 'perfil'>('dashboard');
  menuAberto = signal<boolean>(false);
  graficoDashboard = signal<'diario' | 'semanal' | 'mensal'>('diario');
  subAbaPerfil = signal<'biometrico' | 'fisico' | 'seguranca'>('biometrico');

  // Dados Dashboard
  metaCalorias = signal<number>(0);
  caloriasConsumidas = signal<number>(0);
  caloriasQueimadas = signal<number>(0);
  refeicoesDeHoje = signal<any[]>([]);
  exerciciosDeHoje = signal<any[]>([]);
  dadosGraficoSemanal = signal<any[]>([]);
  dadosGraficoMensal = signal<any[]>([]);

  // Modais
  mostrarModalRefeicao = signal<boolean>(false);
  mostrarModalExercicio = signal<boolean>(false);
  imagemRefeicao = signal<string | null>(null);

  // Formulário Perfil
  perfilId = signal<number>(0);
  dataNascimento = signal<string>('');
  genero = signal<number>(1);
  alturaCm = signal<number>(0);
  nivelAtividade = signal<number>(1);
  objetivo = signal<number>(1);

  // Formulário Registro Físico
  registroFisicoId = signal<number>(0);
  pesoKg = signal<number>(0);
  metaCaloricaDiaria = signal<number>(0);

  // Segurança
  novaSenha = signal<string>('');
  confirmarNovaSenha = signal<string>('');

  todayDate = new Date().toISOString().split('T')[0];

  ngOnInit(): void {
    this.obterGraficoDiario();
  }

  alterarAbaMenu(aba: 'dashboard' | 'estatisticas' | 'perfil') {
    this.abaMenu.set(aba);
    if (aba === 'perfil') {
      this.carregarDadosPerfil();
    }
  }

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

  carregarDadosPerfil() {
    const userId = this.autenticacao.obterId();
    
    this.perfilService.obterPorUsuarioId(userId).subscribe({
      next: (res: any) => {
        this.perfilId.set(res.id);
        this.dataNascimento.set(res.dataNascimento.split('T')[0]);
        this.genero.set(res.genero);
        this.alturaCm.set(res.alturaCm);
        this.nivelAtividade.set(res.nivelAtividade);
        this.objetivo.set(res.objetivo);
      }
    });

    this.registroFisicoService.obterUltimoPorUsuarioId(userId).subscribe({
      next: (res: any) => {
        this.registroFisicoId.set(res.id);
        this.pesoKg.set(res.pesoKg);
        this.metaCaloricaDiaria.set(res.metaCaloricaDiaria);
      }
    });
  }

  salvarPerfilBiometrico() {
    const dados = {
      usuarioId: this.autenticacao.obterId(),
      dataNascimento: this.dataNascimento(),
      genero: Number(this.genero()),
      alturaCm: Number(this.alturaCm()),
      nivelAtividade: Number(this.nivelAtividade()),
      objetivo: Number(this.objetivo())
    };

    this.perfilService.atualizar(dados.usuarioId, dados).subscribe({
      next: () => {
        Swal.fire({ title: 'Sucesso!', text: 'Perfil atualizado.', icon: 'success', confirmButtonColor: '#10b981' });
        this.obterGraficoDiario();
      },
      error: () => Swal.fire({ title: 'Erro!', text: 'Falha ao atualizar perfil.', icon: 'error' })
    });
  }

  salvarRegistroFisico() {
    const dados = {
      usuarioId: this.autenticacao.obterId(),
      pesoKg: Number(this.pesoKg()),
      metaCaloricaDiaria: Number(this.metaCaloricaDiaria())
    };

    this.registroFisicoService.atualizar(dados.usuarioId, dados).subscribe({
      next: () => {
        Swal.fire({ title: 'Sucesso!', text: 'Medidas atualizadas.', icon: 'success', confirmButtonColor: '#10b981' });
        this.obterGraficoDiario();
      },
      error: () => Swal.fire({ title: 'Erro!', text: 'Falha ao atualizar medidas.', icon: 'error' })
    });
  }

  salvarNovaSenha() {
    if (!this.novaSenha() || this.novaSenha().length < 6) {
      Swal.fire({ title: 'Atenção', text: 'A senha deve ter no mínimo 6 caracteres.', icon: 'warning' });
      return;
    }

    if (this.novaSenha() !== this.confirmarNovaSenha()) {
      Swal.fire({ title: 'Erro', text: 'As senhas não coincidem.', icon: 'error' });
      return;
    }

    this.usuarioService.atualizarSenha(this.novaSenha()).subscribe({
      next: () => {
        Swal.fire({ title: 'Sucesso!', text: 'Senha atualizada com sucesso.', icon: 'success', confirmButtonColor: '#10b981' });
        this.novaSenha.set('');
        this.confirmarNovaSenha.set('');
      },
      error: () => Swal.fire({ title: 'Erro!', text: 'Falha ao atualizar senha.', icon: 'error' })
    });
  }

  obterGraficoDiario(){
    this.graficoService.obterGraficoDiario(this.autenticacao.obterId())
      .subscribe({
        next: (resposta: any) => {
          this.metaCalorias.set(resposta.metaCaloricaDiaria);
          this.caloriasConsumidas.set(resposta.totalCaloriasConsumidas);
          this.refeicoesDeHoje.set(resposta.refeicoes);
          this.caloriasQueimadas.set(resposta.totalCaloriasGastas);
          this.exerciciosDeHoje.set(resposta.exercicios);
        },
        error: (erro) => console.error('Falha ao obter gráfico diário', erro)
      });
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
            Swal.fire({ title: 'Excluído!', text: 'Sua refeição foi removida.', icon: 'success', confirmButtonColor: '#10b981' });
          },
          error: (erro) => Swal.fire({ title: 'Erro!', text: 'Não foi possível excluir a refeição.', icon: 'error', confirmButtonColor: '#10b981' })
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
            Swal.fire({ title: 'Excluído!', text: 'Seu exercício foi removido.', icon: 'success', confirmButtonColor: '#10b981' });
          },
          error: (erro) => Swal.fire({ title: 'Erro!', text: 'Não foi possível excluir o exercício.', icon: 'error', confirmButtonColor: '#10b981' })
        });
      }
    });
  }

  // Mapas de exibição
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

  formatarTempo(tempo: string | null | undefined): string {
    if (!tempo) return 'Tempo não registrado';
    const partes = tempo.split(':');
    if (partes.length < 2) return tempo;
    const horas = parseInt(partes[0], 10);
    const minutos = parseInt(partes[1], 10);
    let texto = 'Tempo:';
    if (horas > 0) texto += ` ${horas} hora${horas > 1 ? 's' : ''}`;
    if (minutos > 0) {
      if (horas > 0) texto += ' e ';
      texto += `${minutos} min`;
    }
    return texto;
  }
}

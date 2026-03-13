import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Menu } from '../../shared/menu/menu';
import { Ui } from '../../core/services/ui.service';
import { Carregamento } from '../../shared/carregamento/carregamento';
import { AutenticacaoService } from '../../core/services/autenticacao.service';
import { RefeicaoService } from '../../core/services/refeicao.service';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, Carregamento, FormsModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})

export class Dashboard implements OnInit {
  refeicaoService = inject(RefeicaoService);
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

  mapaRefeicoes: Record<number, any> = {
    1: { nome: 'Café da Manhã', icone: '☕', cor: 'bg-orange-100 text-orange-500' },
    2: { nome: 'Almoço',        icone: '🍽️', cor: 'bg-emerald-100 text-emerald-500' },
    3: { nome: 'Jantar',        icone: '🌙', cor: 'bg-blue-100 text-blue-500' },
    4: { nome: 'Lanche',        icone: '🥪', cor: 'bg-purple-100 text-purple-500' }
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
    this.refeicaoService.obterGraficoDiario(this.autenticacao.obterId())
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
      //this.caloriasQueimadas.set(300);
  }

  limparEFecharModal() {
    this.tipoRefeicao.set(1);
    this.dataRefeicao.set(this.todayDate);
    this.pesoRefeicao.set(null);
    this.apelidoRefeicao.set('');
    this.fotoReal.set(null);
    this.closeMealModal();
  }
}
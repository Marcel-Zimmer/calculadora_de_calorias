import { Component, inject, model, signal, output, computed, input, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AutenticacaoService } from '../../core/services/autenticacao.service';
import { AtividadeFisicaService } from '../../core/services/atividade-fisica.service';

@Component({
  selector: 'app-adicionar-exercicio',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './adicionar-exercicio.html',
  styleUrl: './adicionar-exercicio.css',
})
export class AdicionarExercicio {
  
  mostrarModal = model<boolean>(false);
  dataPreSelecionada = input<string>();
  exercicioAdicionado = output<void>();

  autenticacao = inject(AutenticacaoService);
  atividadeFisicaService = inject(AtividadeFisicaService);

  todayDate = new Date().toLocaleDateString('en-CA', { timeZone: 'America/Sao_Paulo' });

  constructor() {
    effect(() => {
      const data = this.dataPreSelecionada();
      if (data) {
        this.dataExercicio.set(data);
      }
    });
  }

  // Variáveis do formulário simplificadas
  tipoExercicio = signal<number>(1);
  dataExercicio = signal<string>(this.todayDate);
  tempoExercicio = signal<string>(''); // O input de tempo retorna "HH:mm"
  calorias = signal<number | null>(null);

  mapaExercicios: Record<number, any> = this.atividadeFisicaService.obterMapaExercicios();
  listaExercicios = signal<any[]>(this.atividadeFisicaService.obterListaExercicios());

  exercicioSelecionado = computed(() => this.mapaExercicios[this.tipoExercicio()]);

  fecharModal() {
    this.mostrarModal.set(false);
  }

  salvarExercicio() {
    const tempoReal = this.tempoExercicio();
    const tempoFormatado = tempoReal ? `${tempoReal}:00` : '00:00:00';

    const request = {
      UsuarioId: this.autenticacao.obterId(),
      Tipo: Number(this.tipoExercicio()),
      DataDoExercicio: this.dataExercicio(),
      TempoDeExercicio: tempoFormatado,
      CaloriasEstimadas: this.calorias() || 0
    };

    this.atividadeFisicaService.adicionar(request).subscribe({
      next: (resposta: any) => {
        this.exercicioAdicionado.emit();
        this.limparEFecharModal();
      },
      error: (erro: any) => {
        console.error('Erro ao salvar o treino', erro);
      }
    });
    this.limparEFecharModal();
  }

  limparEFecharModal() {
    this.tipoExercicio.set(1);
    this.dataExercicio.set(this.todayDate);
    this.tempoExercicio.set('');
    this.calorias.set(null);
    this.fecharModal();
  }
}
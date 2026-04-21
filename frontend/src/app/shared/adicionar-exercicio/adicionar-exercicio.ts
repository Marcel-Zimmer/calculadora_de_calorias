import { Component, inject, model, signal, output, computed } from '@angular/core';
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
  exercicioAdicionado = output<void>();

  autenticacao = inject(AutenticacaoService);
  atividadeFisicaService = inject(AtividadeFisicaService);

  todayDate = new Date().toLocaleDateString('en-CA', { timeZone: 'America/Sao_Paulo' });

  // Variáveis do formulário simplificadas
  tipoExercicio = signal<number>(1);
  dataExercicio = signal<string>(this.todayDate);
  tempoExercicio = signal<string>(''); // O input de tempo retorna "HH:mm"
  calorias = signal<number | null>(null);

  mapaExercicios: Record<number, any> = {
      1: { nome: 'Ciclismo',   icone: '🚴', cor: 'bg-sky-100 text-sky-600', border: 'border-sky-100', text: 'text-sky-800' },
      2: { nome: 'Boxe',       icone: '🥊', cor: 'bg-red-100 text-red-600', border: 'border-red-100', text: 'text-red-800' },
      3: { nome: 'Musculação', icone: '🏋️', cor: 'bg-slate-100 text-slate-600', border: 'border-slate-100', text: 'text-slate-800' },
      4: { nome: 'Corrida',    icone: '🏃', cor: 'bg-amber-100 text-amber-600', border: 'border-amber-100', text: 'text-amber-800' },
      5: { nome: 'Natação',    icone: '🏊', cor: 'bg-cyan-100 text-cyan-600', border: 'border-cyan-100', text: 'text-cyan-800' }
  };

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
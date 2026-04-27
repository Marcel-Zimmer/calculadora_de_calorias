import { Component, input, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DetalheRefeicaoComponent } from '../detalhe-refeicao/detalhe-refeicao';
import { EditarExercicioComponent } from '../editar-exercicio/editar-exercicio';

@Component({
  selector: 'app-grafico-diario',
  standalone: true,
  imports: [CommonModule, DetalheRefeicaoComponent, EditarExercicioComponent],
  templateUrl: './grafico-diario.html'
})
export class GraficoDiario {
  refeicoes = input<any[]>([]);
  exercicios = input<any[]>([]);
  mapaRefeicoes = input<Record<number, any>>({});
  mapaExercicios = input<Record<number, any>>({});

  onRefeicaoAlterada = output<void>();
  onExercicioAlterado = output<void>();

  refeicaoSelecionada = signal<any>(null);
  mostrarModalRefeicao = signal<boolean>(false);

  exercicioSelecionado = signal<any>(null);
  mostrarModalExercicio = signal<boolean>(false);

  abrirDetalheRefeicao(refeicao: any) {
    this.refeicaoSelecionada.set(refeicao);
    this.mostrarModalRefeicao.set(true);
  }

  abrirEditarExercicio(exercicio: any) {
    this.exercicioSelecionado.set(exercicio);
    this.mostrarModalExercicio.set(true);
  }

  refeicaoAlterada() {
    this.onRefeicaoAlterada.emit();
  }

  exercicioAlterado() {
    this.onExercicioAlterado.emit();
  }

  formatarTempo(tempo: string | null | undefined): string {
    if (!tempo) return 'Tempo não registrado';
    const partes = tempo.split(':');
    if (partes.length < 2) return tempo;

    const horas = parseInt(partes[0], 10);
    const minutos = parseInt(partes[1], 10);

    let texto = 'Tempo:';
    if (horas > 0) texto += ` ${horas} hora${horas > 1 ? 's' : ''}`;
    if (minutos > 0) {
      if (horas > 0) texto += ' e';
      texto += ` ${minutos} min`;
    }
    if (horas === 0 && minutos === 0) return 'Tempo: Menos de 1 min';
    return texto;
  }
}

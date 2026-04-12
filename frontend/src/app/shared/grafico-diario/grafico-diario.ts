import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-grafico-diario',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './grafico-diario.html'
})
export class GraficoDiario {
  refeicoes = input<any[]>([]);
  exercicios = input<any[]>([]);
  mapaRefeicoes = input<Record<number, any>>({});
  mapaExercicios = input<Record<number, any>>({});

  onExcluirRefeicao = output<number>();
  onExcluirExercicio = output<number>();

  excluirRefeicao(id: number) {
    this.onExcluirRefeicao.emit(id);
  }

  excluirExercicio(id: number) {
    this.onExcluirExercicio.emit(id);
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
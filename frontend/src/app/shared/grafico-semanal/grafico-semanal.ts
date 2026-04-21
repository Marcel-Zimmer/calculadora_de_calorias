import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-grafico-semanal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './grafico-semanal.html'
})
export class GraficoSemanal {
  meta = input<number>(0);
  dados = input<any[]>([]);
  tipoVisualizacao = input<'consumo' | 'gasto'>('consumo');
}
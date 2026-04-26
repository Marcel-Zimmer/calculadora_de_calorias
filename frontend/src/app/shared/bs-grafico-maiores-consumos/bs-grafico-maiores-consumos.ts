import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-bs-grafico-maiores-consumos',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bs-grafico-maiores-consumos.html'
})
export class BsGraficoMaioresConsumosComponent {
  titulo = input<string>('Maiores Consumos');
  corBarra = input<string>('bg-indigo-400');
  picos = input.required<any[]>(); // { legenda: string, valor: number }
  maximo = input.required<number>();
}

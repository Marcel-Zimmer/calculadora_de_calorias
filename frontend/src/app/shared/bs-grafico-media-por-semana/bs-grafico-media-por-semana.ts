import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-bs-grafico-media-por-semana',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bs-grafico-media-por-semana.html'
})
export class BsGraficoMediaPorSemanaComponent {
  titulo = input<string>('Média por Semana');
  corBarra = input<string>('bg-indigo-400');
  corHover = input<string>('hover:bg-indigo-500');
  semanas = input.required<any[]>(); // { nome: string, valor: number }
}

import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-grafico-mensal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './grafico-mensal.html'
})
export class GraficoMensal {
  meta = input<number>(0);
  dados = input<any[]>([]);
}
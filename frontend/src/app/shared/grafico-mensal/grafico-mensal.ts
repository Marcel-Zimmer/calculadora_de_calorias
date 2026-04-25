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
  tipoVisualizacao = input<'consumo' | 'gasto' | 'liquido'>('consumo');

  obterValor(day: any): number {
    if (this.tipoVisualizacao() === 'gasto') return day.caloriasGastas;
    if (this.tipoVisualizacao() === 'liquido') return Math.max(0, day.caloriasConsumidas - day.caloriasGastas);
    return day.caloriasConsumidas;
  }

  obterLargura(day: any): string {
    const valor = this.obterValor(day);
    if (valor === 0) return '0%';
    const percentual = (valor / 3000) * 100;
    return `${Math.min(100, percentual)}%`;
  }

  obterCor(day: any): string {
    const valor = this.obterValor(day);
    if (this.tipoVisualizacao() === 'gasto') return 'bg-orange-400';
    return valor > this.meta() ? 'bg-rose-400' : 'bg-emerald-400';
  }
}
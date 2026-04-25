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
  tipoVisualizacao = input<'consumo' | 'gasto' | 'liquido'>('consumo');

  obterValor(day: any): number {
    if (this.tipoVisualizacao() === 'gasto') return day.caloriasGastas;
    if (this.tipoVisualizacao() === 'liquido') return Math.max(0, day.caloriasConsumidas - day.caloriasGastas);
    return day.caloriasConsumidas;
  }

  obterAltura(day: any): string {
    const valor = this.obterValor(day);
    if (valor === 0) return '4px';
    const percentual = (valor / 3000) * 100;
    return `${Math.min(100, percentual)}%`;
  }

  obterCor(day: any): string {
    const valor = this.obterValor(day);
    if (this.tipoVisualizacao() === 'gasto') return 'bg-orange-400';
    
    // Para consumo ou líquido, muda de cor se passar da meta
    return valor > this.meta() ? 'bg-rose-400' : 'bg-emerald-400';
  }
}
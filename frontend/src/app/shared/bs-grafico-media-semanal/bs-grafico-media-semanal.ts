import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface DadoHistorico {
  legenda: string | number;
  valor: number;
}

@Component({
  selector: 'app-bs-grafico-media-semanal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bs-grafico-media-semanal.html',
  styleUrl: './bs-grafico-media-semanal.css'
})
export class BsGraficoMediaSemanalComponent {
  titulo = input.required<string>();
  dados = input.required<DadoHistorico[]>();
  metaDiaria = input<number>(0);
  pintarPorMeta = input<boolean>(false);
  corPadrao = input<string>('bg-emerald-400');

  obterAltura(valor: number): string {
    if (valor <= 0) return '4px';
    // Baseado em 3000 como máximo para manter o padrão visual anterior
    const percentual = (valor / 3000) * 100;
    return `${Math.min(100, percentual)}%`;
  }

  obterClasseCor(valor: number): string {
    if (this.pintarPorMeta()) {
      return valor > this.metaDiaria() ? 'bg-rose-400' : 'bg-emerald-400';
    }
    return this.corPadrao();
  }

  formatarLegenda(legenda: string | number): string {
    const texto = legenda.toString();
    return texto.length > 3 ? texto.substring(0, 3) : texto;
  }
}

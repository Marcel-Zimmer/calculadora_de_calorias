import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

export interface DadoHistorico {
  legenda: string | number;
  valor: number;
}

@Component({
  selector: 'app-bs-grafico-historico-mensal',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bs-grafico-historico-mensal.html',
  styleUrl: './bs-grafico-historico-mensal.css'
})
export class BsGraficoHistoricoMensalComponent {
  titulo = input.required<string>();
  dados = input.required<DadoHistorico[]>();
  metaDiaria = input<number>(0);
  pintarPorMeta = input<boolean>(false);

  obterClasseCor(valor: number): string {
    if (valor === 0) return 'text-slate-400';

    if (this.pintarPorMeta()) {
      return valor > this.metaDiaria() ? 'text-rose-500' : 'text-emerald-500';
    }

    return 'text-indigo-500';
  }
}

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
    if (valor === 0) return 'texto-vazio';

    if (this.pintarPorMeta()) {
      return valor > this.metaDiaria() ? 'texto-vermelho' : 'texto-verde';
    }

    return 'texto-laranja';
  }
}

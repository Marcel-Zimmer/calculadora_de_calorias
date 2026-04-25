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
  corPadrao = input<string>('bg-indigo-500');

  obterAltura(valor: number): string {
    const valorAbsoluto = Math.abs(valor);
    
    // Altura total disponível é aprox 224px (14rem)
    // Deixamos uma margem para o texto no topo
    const alturaMaximaPx = 180;
    
    // Calcula a proporção baseada em 3000 kcal
    const proporcao = valorAbsoluto / 3000;
    const alturaPx = Math.floor(proporcao * alturaMaximaPx);
    
    // Garantimos um mínimo de 8px para que a barra seja sempre visível
    return `${Math.max(8, Math.min(alturaMaximaPx, alturaPx))}px`;
  }

  obterCorBarra(valor: number): string {
    if (valor === 0) return '#cbd5e1'; // slate-300

    if (!this.pintarPorMeta()) {
      return '#6366f1'; // indigo-500
    }

    // emerald-500 : rose-500
    return valor > this.metaDiaria() ? '#f43f5e' : '#10b981';
  }

  formatarLegenda(legenda: string | number): string {
    const texto = legenda.toString();
    return texto.length > 3 ? texto.substring(0, 3) : texto;
  }
}

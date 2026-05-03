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

  obterAltura(valor: number | null | undefined): string {
    const valorAbsoluto = Math.abs(valor || 0);
    
    // Altura total disponível no container h-32 é aprox 128px
    // Deixamos uma margem para o texto no topo
    const alturaMaximaPx = 100;
    
    // Calcula a proporção baseada em 3000 kcal
    const proporcao = valorAbsoluto / 3000;
    const alturaPx = Math.floor(proporcao * alturaMaximaPx);
    
    // Garantimos um mínimo de 8px para que a barra seja sempre visível (cor cinza)
    return `${Math.max(8, Math.min(alturaMaximaPx, alturaPx))}px`;
  }

  obterCorBarra(valor: number | null | undefined): string {
    if (!valor || valor === 0) return '#cbd5e1'; // slate-300

    // Se for bg-indigo-400, retornamos o hex correspondente
    if (this.corBarra() === 'bg-indigo-400') {
      return '#818cf8';
    }

    return '#6366f1'; // indigo-500 default
  }
}

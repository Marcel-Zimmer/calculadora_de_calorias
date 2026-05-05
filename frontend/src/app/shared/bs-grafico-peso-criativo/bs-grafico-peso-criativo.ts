import { Component, input, computed, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PesoPontoResponse } from '../../core/models/grafico.interface';

@Component({
  selector: 'app-bs-grafico-peso-criativo',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bs-grafico-peso-criativo.html',
  styles: [`
    .journey-path {
      stroke-dasharray: 1000;
      stroke-dashoffset: 1000;
      animation: draw 3s ease-out forwards;
    }
    
    .path-bg { stroke-width: 6; }
    .path-main { stroke-width: 3; }

    @media (max-width: 768px) {
      .path-bg { stroke-width: 4; }
      .path-main { stroke-width: 2; }
    }

    @keyframes draw {
      to { stroke-dashoffset: 0; }
    }

    /* Correção do Eixo: Animação e Transform Origin */
    .node-group circle, .node-group text {
       transition: all 0.3s ease-in-out;
       transform-origin: center;
       transform-box: fill-box;
    }

    .node-group:hover .main-circle {
       transform: scale(1.2);
    }

    .node-bounce {
      animation: bounce 3s infinite ease-in-out;
    }

    @keyframes bounce {
      0%, 100% { transform: translateY(0); }
      50% { transform: translateY(-8px); }
    }

    .hide-scrollbar::-webkit-scrollbar { display: none; }
    .hide-scrollbar { -ms-overflow-style: none; scrollbar-width: none; }
  `]
})
export class BsGraficoPesoCriativoComponent {
  dados = input.required<PesoPontoResponse[]>();
  pontoSelecionado = output<PesoPontoResponse>();

  // --- Lógica Horizontal (Desktop/Tablet) ---
  caminhoSvg = computed(() => {
    const pontos = this.dados();
    if (pontos.length < 2) return '';
    const width = 800; const height = 200; const padding = 50;
    const step = (width - padding * 2) / (pontos.length - 1);
    const minPeso = Math.min(...pontos.map(p => p.peso)) - 2;
    const maxPeso = Math.max(...pontos.map(p => p.peso)) + 2;
    const range = maxPeso - minPeso;

    let path = `M ${padding} ${this.getY(pontos[0].peso, height, padding, minPeso, range)}`;
    for (let i = 1; i < pontos.length; i++) {
      const x = padding + i * step;
      const y = this.getY(pontos[i].peso, height, padding, minPeso, range);
      const prevX = padding + (i - 1) * step;
      const prevY = this.getY(pontos[i-1].peso, height, padding, minPeso, range);
      path += ` C ${prevX + step / 2} ${prevY}, ${prevX + step / 2} ${y}, ${x} ${y}`;
    }
    return path;
  });

  nosCalculados = computed(() => {
    const pontos = this.dados();
    const width = 800; const height = 200; const padding = 50;
    const step = pontos.length > 1 ? (width - padding * 2) / (pontos.length - 1) : 0;
    const minPeso = Math.min(...pontos.map(p => p.peso)) - 2;
    const maxPeso = Math.max(...pontos.map(p => p.peso)) + 2;
    const range = maxPeso - minPeso || 1;

    return pontos.map((p, i) => ({
      id: p.id,
      x: padding + i * step,
      y: this.getY(p.peso, height, padding, minPeso, range),
      peso: p.peso,
      legenda: p.legenda,
      cor: this.obterCorTendencia(p.peso, i > 0 ? pontos[i-1].peso : null)
    }));
  });

  // --- Lógica Vertical (Mobile) ---
  caminhoVerticalSvg = computed(() => {
    const pontos = this.dados();
    if (pontos.length < 2) return '';
    const stepY = 100;
    const centerX = 100;
    let path = `M ${centerX} 50`;
    for (let i = 1; i < pontos.length; i++) {
        const x = centerX + (i % 2 === 0 ? 20 : -20);
        const y = 50 + i * stepY;
        const prevX = centerX + ((i-1) % 2 === 0 ? 20 : -20);
        const prevY = 50 + (i-1) * stepY;
        path += ` C ${prevX} ${prevY + stepY/2}, ${x} ${prevY + stepY/2}, ${x} ${y}`;
    }
    return path;
  });

  nosVerticalCalculados = computed(() => {
    const pontos = this.dados();
    const stepY = 100;
    const centerX = 100;
    return pontos.map((p, i) => ({
      id: p.id,
      x: centerX + (i % 2 === 0 ? 20 : -20),
      y: 50 + i * stepY,
      peso: p.peso,
      legenda: p.legenda,
      cor: this.obterCorTendencia(p.peso, i > 0 ? pontos[i-1].peso : null)
    }));
  });

  selecionarPonto(no: any) {
    this.pontoSelecionado.emit({
        id: no.id,
        peso: no.peso,
        legenda: no.legenda,
        data: ''
    });
  }

  private getY(peso: number, height: number, padding: number, min: number, range: number): number {
    return height - padding - ((peso - min) / range) * (height - padding * 2);
  }

  private obterCorTendencia(peso: number, anterior: number | null): string {
    if (anterior === null) return '#6366f1';
    if (peso < anterior) return '#10b981';
    if (peso > anterior) return '#f43f5e';
    return '#6366f1';
  }
}

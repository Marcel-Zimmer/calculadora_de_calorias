import { Component, input, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MacrosResponse } from '../../core/services/nutrientes.service';

@Component({
  selector: 'app-bs-grafico-divisao-calorias',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bs-grafico-divisao-calorias.html'
})
export class BsGraficoDivisaoCaloriasComponent {
  macros = input<MacrosResponse | null>(null);

  svgData = computed(() => {
    const m = this.macros();
    if (!m) return null;

    const offsetProt = 0;
    const offsetCarb = m.percentualProteinas;
    const offsetGord = m.percentualProteinas + m.percentualCarboidratos;

    return {
      offsetProt,
      offsetCarb,
      offsetGord
    };
  });
}

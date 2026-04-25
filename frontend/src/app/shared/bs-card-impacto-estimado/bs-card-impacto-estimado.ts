import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-bs-card-impacto-estimado',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bs-card-impacto-estimado.html',
  styleUrl: './bs-card-impacto-estimado.css'
})
export class BsCardImpactoEstimadoComponent {
  impactoPeso = input.required<number>();

  formatarImpacto(valorKg: number): string {
    const absValor = Math.abs(valorKg);
    if (absValor < 1) {
      return `${(absValor * 1000).toFixed(0)}g`;
    }
    return `${absValor.toFixed(2)}kg`;
  }
}

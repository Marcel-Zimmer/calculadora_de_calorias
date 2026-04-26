import { Component, input, computed } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-bs-card-nutrientes',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bs-card-nutrientes.html'
})
export class BsCardNutrientesComponent {
  nome = input.required<string>();
  icone = input.required<string>();
  valor = input.required<number>();
  meta = input.required<number>();
  cor = input<string>('stroke-blue-500');
  bg = input<string>('text-blue-100');
  isLimite = input<boolean>(false);

  percentual = computed(() => {
    if (this.meta() <= 0) return 0;
    return (this.valor() / this.meta()) * 100;
  });

  strokeDasharray = computed(() => {
    const p = Math.min(this.percentual(), 100);
    const circumference = 2 * Math.PI * 16;
    return `${(p / 100) * circumference} ${circumference}`;
  });
}

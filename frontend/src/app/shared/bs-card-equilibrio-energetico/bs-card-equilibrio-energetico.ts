import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-bs-card-equilibrio-energetico',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bs-card-equilibrio-energetico.html',
  styleUrl: './bs-card-equilibrio-energetico.css'
})
export class BsCardEquilibrioEnergeticoComponent {
  diferencaAbsoluta = input.required<number>();
  Math = Math;
}

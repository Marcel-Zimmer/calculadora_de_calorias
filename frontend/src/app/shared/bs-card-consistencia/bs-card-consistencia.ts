import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-bs-card-consistencia',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bs-card-consistencia.html',
  styleUrl: './bs-card-consistencia.css'
})
export class BsCardConsistenciaComponent {
  diasNaMeta = input.required<number>();
  totalDias = input.required<number>();
}

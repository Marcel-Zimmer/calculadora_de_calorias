import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-bs-card-maior-gasto',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bs-card-maior-gasto.html'
})
export class BsCardMaiorGastoComponent {
  maiorGasto = input.required<number>();
  diaMaiorGasto = input.required<string>();
}

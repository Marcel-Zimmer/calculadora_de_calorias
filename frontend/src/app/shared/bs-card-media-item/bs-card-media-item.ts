import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-bs-card-media-item',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bs-card-media-item.html'
})
export class BsCardMediaItemComponent {
  icone = input.required<string>();
  corCss = input.required<string>();
  titulo = input.required<string>();
  valor = input.required<number>();
  legenda = input<string>('média diária');
}

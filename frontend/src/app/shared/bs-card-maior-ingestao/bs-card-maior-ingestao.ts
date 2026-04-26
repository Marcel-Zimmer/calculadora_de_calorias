import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-bs-card-maior-ingestao',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bs-card-maior-ingestao.html'
})
export class BsCardMaiorIngestaoComponent {
  maiorIngestao = input.required<number>();
  diaMaiorIngestao = input.required<string>();
}

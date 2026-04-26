import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VereditoResponse } from '../../core/services/nutrientes.service';

@Component({
  selector: 'app-bs-card-perfil-anabolico',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bs-card-perfil-anabolico.html'
})
export class BsCardPerfilAnabolicoComponent {
  veredito = input<VereditoResponse | null>(null);
}

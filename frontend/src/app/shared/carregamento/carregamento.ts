import { Component, inject } from '@angular/core';
import { CarragamentoService } from '../../core/services/carregamento.service';

@Component({
  selector: 'app-carregamento',
  imports: [], 
  templateUrl: './carregamento.html',
  styleUrl: './carregamento.css',
})
export class Carregamento {
  loading = inject(CarragamentoService);
}
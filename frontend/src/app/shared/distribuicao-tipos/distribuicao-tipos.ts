import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-distribuicao-tipos',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './distribuicao-tipos.html'
})
export class DistribuicaoTipos {
  titulo = input<string>('Distribuição');
  itens = input<any[]>([]); // { nome, valor, percentual }
  corPrincipal = input<string>('bg-emerald-500');
}

import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-bs-grafico-distribuicao-itens',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './bs-grafico-distribuicao-itens.html'
})
export class BsGraficoDistribuicaoItensComponent {
  titulo = input.required<string>();
  itens = input.required<any[]>(); // { displayNome, valor, percentual, icone, corCss }
  corPrincipal = input<string>('bg-indigo-500');
}

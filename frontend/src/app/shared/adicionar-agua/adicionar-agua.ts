import { Component, model, input, output, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AguaService } from '../../core/services/agua.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-adicionar-agua',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './adicionar-agua.html',
})
export class AdicionarAguaComponent {
  mostrarModal = model.required<boolean>();
  dataPreSelecionada = input<string>();
  aguaAdicionada = output<void>();

  private aguaService = inject(AguaService);

  quantidade = 250;
  salvando = signal(false);

  fecharModal() {
    this.mostrarModal.set(false);
  }

  adicionarPreDefinido(valor: number) {
    this.quantidade = valor;
    this.salvar();
  }

  salvar() {
    if (!this.quantidade || this.quantidade <= 0) return;

    this.salvando.set(true);
    this.aguaService.adicionar({
      quantidadeMl: this.quantidade,
      data: this.dataPreSelecionada()
    }).subscribe({
      next: () => {
        Swal.fire({
          toast: true,
          position: 'top-end',
          showConfirmButton: false,
          timer: 3000,
          timerProgressBar: true,
          icon: 'success',
          title: 'Consumo de água registrado!'
        });
        this.aguaAdicionada.emit();
        this.fecharModal();
        this.salvando.set(false);
      },
      error: () => {
        Swal.fire({
          toast: true,
          position: 'top-end',
          showConfirmButton: false,
          timer: 3000,
          timerProgressBar: true,
          icon: 'error',
          title: 'Erro ao registrar água'
        });
        this.salvando.set(false);
      }
    });
  }
}

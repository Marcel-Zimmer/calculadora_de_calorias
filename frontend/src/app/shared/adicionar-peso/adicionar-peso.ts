import { Component, EventEmitter, inject, input, Output, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RegistroFisicoService } from '../../core/services/registro-fisico.service';
import { AutenticacaoService } from '../../core/services/autenticacao.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-adicionar-peso',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './adicionar-peso.html'
})
export class AdicionarPesoComponent {
  private registroFisicoService = inject(RegistroFisicoService);
  private autenticacao = inject(AutenticacaoService);

  mostrarModal = input.required<boolean>();
  @Output() mostrarModalChange = new EventEmitter<boolean>();
  @Output() pesoAdicionado = new EventEmitter<void>();

  peso = signal<number | null>(null);
  metaCalorica = signal<number | null>(null);
  carregando = signal<boolean>(false);

  constructor() {
    effect(() => {
      if (this.mostrarModal()) {
        this.carregarUltimosDados();
      }
    }, { allowSignalWrites: true });
  }

  carregarUltimosDados() {
    const userId = this.autenticacao.obterId();
    this.registroFisicoService.obterUltimoPorUsuarioId(userId).subscribe((res: any) => {
      if (res) {
        this.peso.set(res.pesoKg);
        this.metaCalorica.set(res.metaCaloricaDiaria);
      }
    });
  }

  fechar() {
    this.mostrarModalChange.emit(false);
  }

  salvar() {
    if (!this.peso() || this.peso()! < 20) {
      Swal.fire('Ops!', 'Informe um peso válido.', 'warning');
      return;
    }

    this.carregando.set(true);
    const payload = {
      usuarioId: this.autenticacao.obterId(),
      pesoKg: this.peso(),
      metaCaloricaDiaria: this.metaCalorica()
    };

    this.registroFisicoService.adicionar(payload).subscribe({
      next: () => {
        this.carregando.set(false);
        Swal.fire({
          icon: 'success',
          title: 'Peso registrado!',
          text: 'Seu progresso foi atualizado.',
          timer: 2000,
          showConfirmButton: false
        });
        this.pesoAdicionado.emit();
        this.fechar();
      },
      error: () => {
        this.carregando.set(false);
        Swal.fire('Erro', 'Não foi possível salvar o registro.', 'error');
      }
    });
  }
}

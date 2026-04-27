import { Component, EventEmitter, inject, input, Output, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PerfilBiometricoService } from '../../core/services/perfil-biometrico.service';
import { AutenticacaoService } from '../../core/services/autenticacao.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-atualizar-meta',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './atualizar-meta.html'
})
export class AtualizarMetaComponent {
  private perfilService = inject(PerfilBiometricoService);
  private autenticacao = inject(AutenticacaoService);

  mostrarModal = input.required<boolean>();
  @Output() mostrarModalChange = new EventEmitter<boolean>();
  @Output() metaAtualizada = new EventEmitter<void>();

  perfilAtual = signal<any>(null);
  objetivoSelecionado = signal<number>(3);
  carregando = signal<boolean>(false);

  constructor() {
    effect(() => {
      if (this.mostrarModal()) {
        this.carregarPerfil();
      }
    }, { allowSignalWrites: true });
  }

  carregarPerfil() {
    const userId = this.autenticacao.obterId();
    this.perfilService.obterPorUsuarioId(userId).subscribe((res: any) => {
      this.perfilAtual.set(res);
      this.objetivoSelecionado.set(res.objetivo);
    });
  }

  fechar() {
    this.mostrarModalChange.emit(false);
  }

  salvar() {
    if (!this.perfilAtual()) return;

    this.carregando.set(true);
    const payload = {
      ...this.perfilAtual(),
      objetivo: Number(this.objetivoSelecionado())
    };

    const userId = this.autenticacao.obterId();
    this.perfilService.atualizar(userId, payload).subscribe({
      next: () => {
        this.carregando.set(false);
        Swal.fire({
          icon: 'success',
          title: 'Meta atualizada!',
          text: 'Seu plano foi reajustado com sucesso.',
          timer: 2000,
          showConfirmButton: false
        });
        this.metaAtualizada.emit();
        this.fechar();
      },
      error: () => {
        this.carregando.set(false);
        Swal.fire('Erro', 'Não foi possível atualizar sua meta.', 'error');
      }
    });
  }
}

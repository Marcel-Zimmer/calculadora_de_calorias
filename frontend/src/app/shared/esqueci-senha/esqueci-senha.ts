import { Component, inject, model, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UsuarioService } from '../../core/services/usuario.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-esqueci-senha',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './esqueci-senha.html',
})
export class EsqueciSenhaComponent {
  private usuarioService = inject(UsuarioService);

  mostrarModal = model<boolean>(false);
  email = signal<string>('');
  carregando = signal<boolean>(false);

  fechar() {
    this.mostrarModal.set(false);
  }

  enviar() {
    if (!this.email()) {
      Swal.fire('Atenção', 'Informe seu e-mail.', 'warning');
      return;
    }

    this.carregando.set(true);
    this.usuarioService.esqueciSenha(this.email()).subscribe({
      next: () => {
        this.carregando.set(false);
        Swal.fire('Sucesso', 'Se o e-mail estiver cadastrado, você receberá instruções para redefinir sua senha.', 'success');
        this.fechar();
      },
      error: () => {
        this.carregando.set(false);
        // Por segurança, muitas vezes retornamos sucesso mesmo se o e-mail não existir, 
        // mas aqui vamos tratar como erro genérico para facilitar o dev.
        Swal.fire('Erro', 'Não foi possível processar sua solicitação.', 'error');
      }
    });
  }
}

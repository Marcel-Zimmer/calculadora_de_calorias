import { Component, computed, inject, signal } from '@angular/core';
import { UsuarioLogin } from '../../core/models/usuario.model';
import { FormsModule } from '@angular/forms';
import { UsuarioService } from '../../core/services/usuario.service';
import { Carregamento } from "../../shared/carregamento/carregamento";
import { AutenticacaoService } from '../../core/services/autenticacao.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [FormsModule, Carregamento],
  templateUrl: './login.html',
  styleUrl: './login.css',
})

export class Login {
  usuarioService = inject(UsuarioService);
  autenticacaoService = inject(AutenticacaoService);
  router = inject(Router);

  email = signal<string>('');
  senha = signal<string>('');
  estaLogado = signal<boolean>(false);
  mensagemErro = signal<string | null>(null);

  login() {
    this.mensagemErro.set(null);

    let objeto: UsuarioLogin = {
      email: this.email(),
      senha: this.senha()
    };

    this.usuarioService.fazerLogin(objeto)
      .subscribe({
        next: (resposta: any) => {
          this.autenticacaoService.salvarSessao(
            resposta.id, 
            resposta.accessToken, 
            resposta.refreshToken
          );
          this.router.navigate(['/dashboard']);
        },
        error: (erro) => {
          console.error('Falha no login', erro);
          this.mensagemErro.set(erro);
        }
      });
  }

  criarConta() {
  }

}

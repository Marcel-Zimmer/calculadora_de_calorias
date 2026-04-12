import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UsuarioService } from '../../core/services/usuario.service';
import { Carregamento } from "../../shared/carregamento/carregamento";
import { UsuarioRegistro } from '../../core/models/usuario.model';

@Component({
  selector: 'app-cadastro',
  imports: [FormsModule, Carregamento],
  templateUrl: './cadastro.html',
  styleUrl: './cadastro.css',
})
export class Cadastro {
  usuarioService = inject(UsuarioService);
  router = inject(Router);

  passo = signal<number>(1);
  mensagemErro = signal<string | null>(null);

  // Passo 1: Conta
  nome = signal<string>('');
  email = signal<string>('');
  senha = signal<string>('');
  confirmarSenha = signal<string>('');

  // Passo 2: Perfil Biométrico
  dataNascimento = signal<string>('');
  genero = signal<number>(1); // 1: Masculino, 2: Feminino
  altura = signal<number>(0);

  // Passo 3: Registro Físico e Metas
  peso = signal<number>(0);
  nivelAtividade = signal<number>(1); // 1-5
  objetivo = signal<number>(1); // 1-3
  metaCalorica = signal<number | null>(null);

  proximoPasso() {
    if (this.passo() === 1) {
      if (!this.nome() || !this.email() || !this.senha()) {
        this.mensagemErro.set('Preencha todos os campos obrigatórios.');
        return;
      }
      if (this.senha() !== this.confirmarSenha()) {
        this.mensagemErro.set('As senhas não coincidem.');
        return;
      }
    }

    if (this.passo() < 3) {
      this.mensagemErro.set(null);
      this.passo.update(p => p + 1);
    }
  }

  passoAnterior() {
    if (this.passo() > 1) {
      this.mensagemErro.set(null);
      this.passo.update(p => p - 1);
    }
  }

  finalizarCadastro() {
    this.mensagemErro.set(null);

    if (this.senha() !== this.confirmarSenha()) {
      this.mensagemErro.set('As senhas não coincidem.');
      return;
    }
    
    const objeto: UsuarioRegistro = {
        nome: this.nome(),
        email: this.email(),
        senha: this.senha(),
        dataNascimento: this.dataNascimento(),
        genero: Number(this.genero()),
        alturaCm: Number(this.altura()),
        pesoKg: Number(this.peso()),
        nivelAtividade: Number(this.nivelAtividade()),
        objetivo: Number(this.objetivo()),
        metaCaloricaDiaria: this.metaCalorica() ? Number(this.metaCalorica()) : null
    };

    this.usuarioService.registrar(objeto)
      .subscribe({
        next: () => {
          this.router.navigate(['/login']);
        },
        error: (erro) => {
          console.error('Falha no registro', erro);
          this.mensagemErro.set(erro?.error?.message || 'Falha ao realizar cadastro. Verifique os dados e tente novamente.');
        }
      });
  }

  voltarLogin() {
    this.router.navigate(['/login']);
  }
}

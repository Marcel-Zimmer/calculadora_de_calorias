import { Component, computed, signal } from '@angular/core';
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
  constructor(private usuarioService : UsuarioService, private autenticacaoService:AutenticacaoService, private router: Router){}

  email = signal<string>('');
  senha = signal<string>('');
  isLoggedIn = signal<boolean>(false);
  authMode = signal<'login' | 'signup'>('login');

  activeTab = signal<'dashboard' | 'estatisticas' | 'perfil'>('dashboard');

  
  showMealModal = signal<boolean>(false);
  showExerciseModal = signal<boolean>(false);

  mealImagePreview = signal<string | null>(null);
  todayDate = new Date().toISOString().split('T')[0];
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
          console.log('Logado com sucesso!', resposta);
          this.autenticacaoService.salvarSessao(resposta.id);
          this.router.navigate(['/dashboard']);

            },
          error: (erro) => {
              console.error('Falha no login', erro);
              
              if (erro.status === 400 && erro.error.errors) {
                const validacoes = erro.error.errors;
                const primeiroErro = Object.values(validacoes)[0] as string[]; 
                this.mensagemErro.set(primeiroErro[0]);
                
              } else if (erro.status === 401 || erro.status === 404) {
                this.mensagemErro.set('E-mail ou senha incorretos.');
                
              } else {
                this.mensagemErro.set('Servidor indisponível. Tente novamente mais tarde.');
              }
            }
          });
  }

  toggleAuthMode() {
    this.authMode.update(mode => mode === 'login' ? 'signup' : 'login');
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e: any) => this.mealImagePreview.set(e.target.result);
      reader.readAsDataURL(file);
    }
  }

  closeMealModal() {
    this.showMealModal.set(false);
    this.mealImagePreview.set(null);
  }

}

import { Injectable, signal, computed } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AutenticacaoService {
  usuarioId = signal<string | null>(localStorage.getItem('meu_app_user_id'));

  logado = computed(() => this.usuarioId() !== null);

  salvarSessao(id: string) {
    localStorage.setItem('meu_app_user_id', id); 
    this.usuarioId.set(id); 
  }

  fazerLogout() {
    localStorage.removeItem('meu_app_user_id'); 
    this.usuarioId.set(null); 
  }
}
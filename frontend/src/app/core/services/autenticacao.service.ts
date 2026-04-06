import { Injectable, signal, computed } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AutenticacaoService {
  private stringId = localStorage.getItem('meu_app_user_id') ?? null;
  usuarioId = signal<number | null>(this.stringId ? Number(this.stringId) : 0);

  logado = computed(() => this.usuarioId() !== null);

  salvarSessao(id: number) {
    localStorage.setItem('meu_app_user_id', id.toString()); 
    this.usuarioId.set(id); 
  }

  fazerLogout() {
    localStorage.removeItem('meu_app_user_id'); 
    this.usuarioId.set(null); 
  }

  obterId(){
    this.stringId = localStorage.getItem('meu_app_user_id') ?? null;
    return this.stringId ? Number(this.stringId) : 0; 
  }
}
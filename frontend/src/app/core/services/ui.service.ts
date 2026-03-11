import { Injectable, signal } from '@angular/core';

export type Aba = 'dashboard' | 'estatisticas' | 'perfil';

@Injectable({
  providedIn: 'root',
})

export class Ui {
  readonly menuAberto = signal<boolean>(false);
  
  readonly abaAtual = signal<Aba>('dashboard');

  toggleMenu() {
    this.menuAberto.update(estado => !estado);
  }

  fecharMenu() {
    this.menuAberto.set(false);
  }

  mudarAba(aba: Aba) {
    this.abaAtual.set(aba);
    this.fecharMenu();
  }
}
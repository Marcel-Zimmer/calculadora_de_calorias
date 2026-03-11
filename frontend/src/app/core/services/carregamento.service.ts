import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class CarragamentoService {
  carregando = signal<boolean>(false);

  mostrar() {
    this.carregando.set(true); 
  }

  esconder() {
    this.carregando.set(false); 
  }
}
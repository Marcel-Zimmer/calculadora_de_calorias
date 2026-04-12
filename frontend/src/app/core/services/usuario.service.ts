import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { UsuarioLogin } from '../models/usuario.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class UsuarioService {
  http = inject(HttpClient); 
  
  private readonly baseUrl = `${environment.apiUrl}/Usuario`; 

  fazerLogin(usuario: UsuarioLogin) {
    return this.http.post(`${this.baseUrl}/login`, usuario);
  }

  registrar(usuario: any) {
    return this.http.post(`${this.baseUrl}/registrar`, usuario);
  }

  atualizarSenha(novaSenha: string) {
    return this.http.put(`${this.baseUrl}/atualizar-senha`, JSON.stringify(novaSenha), {
      headers: { 'Content-Type': 'application/json' }
    });
  }

}

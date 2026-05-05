import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { LoginUsuarioResponse, UsuarioLogin, UsuarioRegistro } from '../models/usuario.interface';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UsuarioService {
  http = inject(HttpClient); 
  
  private readonly baseUrl = `${environment.apiUrl}/Usuario`; 

  fazerLogin(usuario: UsuarioLogin): Observable<LoginUsuarioResponse> {
    return this.http.post<LoginUsuarioResponse>(`${this.baseUrl}/login`, usuario);
  }

  registrar(usuario: UsuarioRegistro): Observable<any> {
    return this.http.post(`${this.baseUrl}/registrar`, usuario);
  }

  atualizarSenha(novaSenha: string): Observable<any> {
    return this.http.put(`${this.baseUrl}/atualizar-senha`, JSON.stringify(novaSenha), {
      headers: { 'Content-Type': 'application/json' }
    });
  }

  esqueciSenha(email: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/esqueci-senha`, { email });
  }

}

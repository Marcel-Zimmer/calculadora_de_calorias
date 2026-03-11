import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { UsuarioLogin } from '../models/usuario.model';

@Injectable({
  providedIn: 'root',
})
export class UsuarioService {
  http = inject(HttpClient); 
  
  baseUrl = 'https://localhost:52732/api/Usuario'; 

  fazerLogin(usuario: UsuarioLogin) {
    return this.http.post(`${this.baseUrl}/login`, usuario);
  }

}

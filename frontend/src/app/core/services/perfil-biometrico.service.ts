import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class PerfilBiometricoService {
  http = inject(HttpClient); 
  
  private readonly baseUrl = `${environment.apiUrl}/PerfilBiometrico`; 

  obterPorUsuarioId(usuarioId: number) {
    return this.http.get(`${this.baseUrl}/usuario/${usuarioId}`);
  }

  atualizar(usuarioId: number, perfil: any) {
    return this.http.put(`${this.baseUrl}/usuario/${usuarioId}`, perfil);
  }
}

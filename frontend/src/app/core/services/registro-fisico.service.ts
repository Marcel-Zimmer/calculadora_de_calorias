import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class RegistroFisicoService {
  http = inject(HttpClient); 
  
  private readonly baseUrl = `${environment.apiUrl}/RegistroFisico`; 

  obterUltimoPorUsuarioId(usuarioId: number) {
    return this.http.get(`${this.baseUrl}/usuario/${usuarioId}`);
  }

  atualizar(usuarioId: number, registro: any) {
    return this.http.put(`${this.baseUrl}/usuario/${usuarioId}`, registro);
  }
}

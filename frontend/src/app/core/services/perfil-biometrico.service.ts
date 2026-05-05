import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class PerfilBiometricoService {
  http = inject(HttpClient); 
  
  private readonly baseUrl = `${environment.apiUrl}/PerfilBiometrico`; 

  obter() {
    return this.http.get(`${this.baseUrl}`);
  }

  adicionar(perfil: any) {
    return this.http.post(this.baseUrl, perfil);
  }

  atualizar(perfil: any) {
    return this.http.put(`${this.baseUrl}`, perfil);
  }
}

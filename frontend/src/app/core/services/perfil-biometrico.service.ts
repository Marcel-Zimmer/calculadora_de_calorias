import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { CriarPerfilBiometricoRequest, PerfilBiometricoResponse } from '../models/perfil-biometrico.interface';

@Injectable({
  providedIn: 'root',
})
export class PerfilBiometricoService {
  http = inject(HttpClient); 
  
  private readonly baseUrl = `${environment.apiUrl}/PerfilBiometrico`; 

  obter(): Observable<PerfilBiometricoResponse> {
    return this.http.get<PerfilBiometricoResponse>(`${this.baseUrl}`);
  }

  adicionar(perfil: CriarPerfilBiometricoRequest): Observable<PerfilBiometricoResponse> {
    return this.http.post<PerfilBiometricoResponse>(this.baseUrl, perfil);
  }

  atualizar(perfil: any): Observable<any> {
    return this.http.put(`${this.baseUrl}`, perfil);
  }
}

import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { CriarRegistroFisicoRequest, RegistroFisicoResponse } from '../models/registro-fisico.interface';

@Injectable({
  providedIn: 'root',
})
export class RegistroFisicoService {
  http = inject(HttpClient); 
  
  private readonly baseUrl = `${environment.apiUrl}/RegistroFisico`; 

  obterUltimo(): Observable<RegistroFisicoResponse> {
    return this.http.get<RegistroFisicoResponse>(`${this.baseUrl}/ultimo`);
  }

  adicionar(registro: CriarRegistroFisicoRequest): Observable<RegistroFisicoResponse> {
    return this.http.post<RegistroFisicoResponse>(this.baseUrl, registro);
  }

  atualizar(registro: CriarRegistroFisicoRequest): Observable<RegistroFisicoResponse> {
    return this.http.put<RegistroFisicoResponse>(`${this.baseUrl}`, registro);
  }

  excluir(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { EstatisticasAguaResponse, RegistroAguaRequest, RegistroAguaResponse } from '../models/agua.interface';

@Injectable({
  providedIn: 'root'
})
export class AguaService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/RegistroAgua`;

  adicionar(request: RegistroAguaRequest): Observable<RegistroAguaResponse> {
    return this.http.post<RegistroAguaResponse>(`${this.apiUrl}/adicionar`, request);
  }

  obterDiario(data?: string): Observable<RegistroAguaResponse[]> {
    let params: Record<string, string> = {};
    if (data) {
      params['data'] = data;
    }
    return this.http.get<RegistroAguaResponse[]>(`${this.apiUrl}/diario`, { params });
  }

  obterEstatisticasSemanais(data?: string): Observable<EstatisticasAguaResponse> {
    let params: Record<string, string> = {};
    if (data) {
      params['data'] = data;
    }
    return this.http.get<EstatisticasAguaResponse>(`${this.apiUrl}/estatisticas/semanal`, { params });
  }

  obterEstatisticasMensais(data?: string): Observable<EstatisticasAguaResponse> {
    let params: Record<string, string> = {};
    if (data) {
      params['data'] = data;
    }
    return this.http.get<EstatisticasAguaResponse>(`${this.apiUrl}/estatisticas/mensal`, { params });
  }

  excluir(id: number): Observable<boolean> {
    return this.http.delete<boolean>(`${this.apiUrl}/${id}`);
  }
}

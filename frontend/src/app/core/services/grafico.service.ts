import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';
import { EstatisticasDetalhadasResponse, EstatisticasPesoResponse, GraficoDiarioResponse, GraficoPeriodoResponse } from '../models/grafico.interface';

@Injectable({
  providedIn: 'root',
})
export class GraficoService {
  http = inject(HttpClient); 
  
  private readonly baseUrl = `${environment.apiUrl}/Grafico`; 

  obterGraficoDiario(data?: string): Observable<GraficoDiarioResponse> {
    let url = `${this.baseUrl}/dashboard-diario`;
    if (data) {
      url += `?data=${data}`;
    }
    return this.http.get<GraficoDiarioResponse>(url);
  }

  obterGraficoSemanal(data?: string): Observable<GraficoPeriodoResponse> {
    let url = `${this.baseUrl}/dashboard-semanal`;
    if (data) url += `?data=${data}`;
    return this.http.get<GraficoPeriodoResponse>(url);
  }

  obterGraficoMensal(data?: string): Observable<GraficoPeriodoResponse> {
    let url = `${this.baseUrl}/dashboard-mensal`;
    if (data) url += `?data=${data}`;
    return this.http.get<GraficoPeriodoResponse>(url);
  }

  obterEstatisticasSemanais(data?: string): Observable<EstatisticasDetalhadasResponse> {
    let url = `${this.baseUrl}/estatisticas-semanais`;
    if (data) url += `?data=${data}`;
    return this.http.get<EstatisticasDetalhadasResponse>(url);
  }

  obterEstatisticasMensais(data?: string): Observable<EstatisticasDetalhadasResponse> {
    let url = `${this.baseUrl}/estatisticas-mensais`;
    if (data) url += `?data=${data}`;
    return this.http.get<EstatisticasDetalhadasResponse>(url);
  }

  obterEstatisticasPeso(): Observable<EstatisticasPesoResponse> {
    return this.http.get<EstatisticasPesoResponse>(`${this.baseUrl}/estatisticas-peso`);
  }

}

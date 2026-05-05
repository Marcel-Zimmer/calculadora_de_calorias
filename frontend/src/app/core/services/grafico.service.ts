import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class GraficoService {
  http = inject(HttpClient); 
  
  private readonly baseUrl = `${environment.apiUrl}/Grafico`; 

  obterGraficoDiario(data?: string) {
    let url = `${this.baseUrl}/dashboard-diario`;
    if (data) {
      url += `?data=${data}`;
    }
    return this.http.get(url);
  }

  obterGraficoSemanal(data?: string) {
    let url = `${this.baseUrl}/dashboard-semanal`;
    if (data) url += `?data=${data}`;
    return this.http.get(url);
  }

  obterGraficoMensal(data?: string) {
    let url = `${this.baseUrl}/dashboard-mensal`;
    if (data) url += `?data=${data}`;
    return this.http.get(url);
  }

  obterEstatisticasSemanais(data?: string) {
    let url = `${this.baseUrl}/estatisticas-semanais`;
    if (data) url += `?data=${data}`;
    return this.http.get(url);
  }

  obterEstatisticasMensais(data?: string) {
    let url = `${this.baseUrl}/estatisticas-mensais`;
    if (data) url += `?data=${data}`;
    return this.http.get(url);
  }

  obterEstatisticasPeso() {
    return this.http.get(`${this.baseUrl}/estatisticas-peso`);
  }

}

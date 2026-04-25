import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class GraficoService {
  http = inject(HttpClient); 
  
  private readonly baseUrl = `${environment.apiUrl}/Grafico`; 

  obterGraficoDiario(usuarioId: number, data?: string) {
    let url = `${this.baseUrl}/dashboard-diario/${usuarioId}`;
    if (data) {
      url += `?data=${data}`;
    }
    return this.http.get(url);
  }

  obterGraficoSemanal(usuarioId: number, data?: string) {
    let url = `${this.baseUrl}/dashboard-semanal/${usuarioId}`;
    if (data) url += `?data=${data}`;
    return this.http.get(url);
  }

  obterGraficoMensal(usuarioId: number, data?: string) {
    let url = `${this.baseUrl}/dashboard-mensal/${usuarioId}`;
    if (data) url += `?data=${data}`;
    return this.http.get(url);
  }

  obterEstatisticasSemanais(usuarioId: number, data?: string) {
    let url = `${this.baseUrl}/estatisticas-semanais/${usuarioId}`;
    if (data) url += `?data=${data}`;
    return this.http.get(url);
  }

  obterEstatisticasMensais(usuarioId: number, data?: string) {
    let url = `${this.baseUrl}/estatisticas-mensais/${usuarioId}`;
    if (data) url += `?data=${data}`;
    return this.http.get(url);
  }

}

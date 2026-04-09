import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class GraficoService {
  http = inject(HttpClient); 
  
  private readonly baseUrl = `${environment.apiUrl}/Grafico`; 

  obterGraficoDiario(usuarioId:number){
    return this.http.get(`${this.baseUrl}/dashboard-diario/`+ usuarioId);
  }

}

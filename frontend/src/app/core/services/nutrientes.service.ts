import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

export interface NutrientesResponse {
  periodo: string;
  metaProteinas: number;
  consumoProteinas: number;
  metaCarboidratos: number;
  consumoCarboidratos: number;
  metaGorduras: number;
  consumoGorduras: number;
  metaFibras: number;
  consumoFibras: number;
  limiteAcucares: number;
  consumoAcucares: number;
}

@Injectable({
  providedIn: 'root'
})
export class NutrientesService {
  private http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/Nutrientes`;

  obterNutrientesDiario(usuarioId: number, data?: string): Observable<NutrientesResponse> {
    let url = `${this.baseUrl}/diario/${usuarioId}`;
    if (data) url += `?data=${data}`;
    return this.http.get<NutrientesResponse>(url);
  }

  obterNutrientesSemanal(usuarioId: number, data?: string): Observable<NutrientesResponse> {
    let url = `${this.baseUrl}/semanal/${usuarioId}`;
    if (data) url += `?data=${data}`;
    return this.http.get<NutrientesResponse>(url);
  }

  obterNutrientesMensal(usuarioId: number, data?: string): Observable<NutrientesResponse> {
    let url = `${this.baseUrl}/mensal/${usuarioId}`;
    if (data) url += `?data=${data}`;
    return this.http.get<NutrientesResponse>(url);
  }
}

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

  obterNutrientesDiario(usuarioId: number): Observable<NutrientesResponse> {
    return this.http.get<NutrientesResponse>(`${this.baseUrl}/diario/${usuarioId}`);
  }

  obterNutrientesSemanal(usuarioId: number): Observable<NutrientesResponse> {
    return this.http.get<NutrientesResponse>(`${this.baseUrl}/semanal/${usuarioId}`);
  }

  obterNutrientesMensal(usuarioId: number): Observable<NutrientesResponse> {
    return this.http.get<NutrientesResponse>(`${this.baseUrl}/mensal/${usuarioId}`);
  }
}

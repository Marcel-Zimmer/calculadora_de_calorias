import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { NutrientesEnum } from '../models/nutrientes.enum';

export interface NutrienteDetalhe {
  tipo: number;
  nome: string;
  valor: number;
  meta: number;
  isLimite: boolean;
}

export interface MacrosResponse {
  caloriasProteinas: number;
  caloriasCarboidratos: number;
  caloriasGorduras: number;
  totalCalorias: number;
  percentualProteinas: number;
  percentualCarboidratos: number;
  percentualGorduras: number;
}

export interface VereditoResponse {
  titulo: string;
  descricao: string;
  icone: string;
  corCss: string;
}

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
  detalhes: NutrienteDetalhe[];
  macros?: MacrosResponse;
  veredito?: VereditoResponse;
}

@Injectable({
  providedIn: 'root'
})
export class NutrientesService {
  private http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/Nutrientes`;

  obterMapaNutrientes(): Record<number, any> {
    return {
      [NutrientesEnum.Proteina]: { nome: 'Proteína', icone: '🥩', cor: 'stroke-rose-500', bg: 'text-rose-100', isLimite: false },
      [NutrientesEnum.Carboidrato]: { nome: 'Carbo', icone: '🍞', cor: 'stroke-amber-400', bg: 'text-amber-100', isLimite: false },
      [NutrientesEnum.Gordura]: { nome: 'Gordura', icone: '🥑', cor: 'stroke-indigo-500', bg: 'text-indigo-100', isLimite: false },
      [NutrientesEnum.Fibra]: { nome: 'Fibras', icone: '🌾', cor: 'stroke-emerald-500', bg: 'text-emerald-100', isLimite: false },
      [NutrientesEnum.Acucar]: { nome: 'Açúcar', icone: '🍭', cor: 'stroke-purple-500', bg: 'text-purple-100', isLimite: true }
    };
  }

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

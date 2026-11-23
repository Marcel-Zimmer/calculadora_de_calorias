import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { NutrientesEnum } from '../models/nutrientes.enum';
import { NutrientesResponse } from '../models/nutrientes.interface';

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

  obterNutrientesDiario(data?: string): Observable<NutrientesResponse> {
    let url = `${this.baseUrl}/diario`;
    if (data) url += `?data=${data}`;
    return this.http.get<NutrientesResponse>(url);
  }

  obterNutrientesSemanal(data?: string): Observable<NutrientesResponse> {
    let url = `${this.baseUrl}/semanal`;
    if (data) url += `?data=${data}`;
    return this.http.get<NutrientesResponse>(url);
  }

  obterNutrientesMensal(data?: string): Observable<NutrientesResponse> {
    let url = `${this.baseUrl}/mensal`;
    if (data) url += `?data=${data}`;
    return this.http.get<NutrientesResponse>(url);
  }
}

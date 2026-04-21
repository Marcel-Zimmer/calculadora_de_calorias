import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { UsuarioLogin } from '../models/usuario.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AtividadeFisicaService {

  http = inject(HttpClient); 
  
  private readonly baseUrl = `${environment.apiUrl}/AtividadeFisica`; 
  
  adicionar(atividade: any) {
    return this.http.post(`${this.baseUrl}`, atividade);
  }

  obterMapaExercicios(): Record<number, any> {
    return {
      1: { id: 1, nome: 'Ciclismo',   icone: '🚴', cor: 'bg-sky-100 text-sky-600', border: 'border-sky-100', text: 'text-sky-800' },
      2: { id: 2, nome: 'Caminhada',  icone: '🚶', cor: 'bg-emerald-100 text-emerald-600', border: 'border-emerald-100', text: 'text-emerald-800' },
      3: { id: 3, nome: 'Corrida',    icone: '🏃', cor: 'bg-amber-100 text-amber-600', border: 'border-amber-100', text: 'text-amber-800' },
      4: { id: 4, nome: 'Boxe',       icone: '🥊', cor: 'bg-red-100 text-red-600', border: 'border-red-100', text: 'text-red-800' },
      5: { id: 5, nome: 'Musculação', icone: '🏋️', cor: 'bg-slate-100 text-slate-600', border: 'border-slate-100', text: 'text-slate-800' }
    };
  }

  obterListaExercicios(): any[] {
    return Object.values(this.obterMapaExercicios());
  }

  excluir(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

}

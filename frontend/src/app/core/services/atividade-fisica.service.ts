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
      1: { id: 1, nome: 'Ciclismo',   icone: '🚴', cor: 'bg-blue-50 text-blue-500' },
      2: { id: 2, nome: 'Caminhada',  icone: '🚶', cor: 'bg-emerald-50 text-emerald-500' },
      3: { id: 3, nome: 'Corrida',    icone: '🏃', cor: 'bg-orange-50 text-orange-500' },
      4: { id: 4, nome: 'Futebol',    icone: '⚽', cor: 'bg-green-50 text-green-500' },
      5: { id: 5, nome: 'Musculação', icone: '🏋️', cor: 'bg-indigo-50 text-indigo-500' },
      6: { id: 6, nome: 'Natação',    icone: '🏊', cor: 'bg-cyan-50 text-cyan-500' },
      7: { id: 7, nome: 'Crossfit',   icone: '💪', cor: 'bg-rose-50 text-rose-500' }
    };
  }

  obterListaExercicios(): any[] {
    return Object.values(this.obterMapaExercicios());
  }

  excluir(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

}

import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class RefeicaoService {
  http = inject(HttpClient); 
  
  private readonly baseUrl = `${environment.apiUrl}/Refeicao`; 

  adicionar(refeicao: FormData) {
    return this.http.post(`${this.baseUrl}/adicionar`, refeicao);
  }

  obterModelosFrequentes(usuarioId: number) {
    return this.http.get<any[]>(`${this.baseUrl}/modelos-frequentes/${usuarioId}`);
  }

  obterMapaRefeicoes(): Record<number, any> {
    return {
      1: { id: 1, nome: 'Café da Manhã', icone: '☕', cor: 'bg-amber-50 text-amber-500' },
      2: { id: 2, nome: 'Almoço',        icone: '🍛', cor: 'bg-emerald-50 text-emerald-500' },
      3: { id: 3, nome: 'Jantar',        icone: '🍽️', cor: 'bg-indigo-50 text-indigo-500' },
      4: { id: 4, nome: 'Lanche',        icone: '🍎', cor: 'bg-orange-50 text-orange-500' },
      5: { id: 5, nome: 'Gula',          icone: '🍕', cor: 'bg-rose-50 text-rose-500' }
    };
  }

  obterListaRefeicoes(): any[] {
    return Object.values(this.obterMapaRefeicoes());
  }

  excluir(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}

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

  excluir(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}

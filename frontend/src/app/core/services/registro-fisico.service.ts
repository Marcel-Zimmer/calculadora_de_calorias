import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class RegistroFisicoService {
  http = inject(HttpClient); 
  
  private readonly baseUrl = `${environment.apiUrl}/RegistroFisico`; 

  obterUltimo() {
    return this.http.get(`${this.baseUrl}/ultimo`);
  }

  adicionar(registro: any) {
    return this.http.post(this.baseUrl, registro);
  }

  atualizar(registro: any) {
    return this.http.put(`${this.baseUrl}`, registro);
  }

  excluir(id: number) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}

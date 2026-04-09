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

}

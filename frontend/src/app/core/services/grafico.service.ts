import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { UsuarioLogin } from '../models/usuario.model';

@Injectable({
  providedIn: 'root',
})
export class GraficoService {
  http = inject(HttpClient); 
  
  baseUrl = 'https://localhost:52732/api/Grafico'; 

  obterGraficoDiario(usuarioId:number){
    return this.http.get(`${this.baseUrl}/dashboard-diario/`+ usuarioId);
  }

}

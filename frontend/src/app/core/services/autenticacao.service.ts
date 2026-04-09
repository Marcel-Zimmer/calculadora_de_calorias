import { Injectable, signal, computed, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AutenticacaoService {
  private http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/Usuario`; 

  private stringId = localStorage.getItem('meu_app_user_id') ?? null;
  usuarioId = signal<number | null>(this.stringId ? Number(this.stringId) : null);

  logado = computed(() => this.usuarioId() !== null);

  salvarSessao(id: number, accessToken: string, refreshToken: string) {
    localStorage.setItem('meu_app_user_id', id.toString());
    localStorage.setItem('meu_token_jwt', accessToken);
    localStorage.setItem('meu_refresh_token', refreshToken);
    this.usuarioId.set(id);
  }

  fazerLogout() {
    localStorage.removeItem('meu_app_user_id');
    localStorage.removeItem('meu_token_jwt');
    localStorage.removeItem('meu_refresh_token');
    this.usuarioId.set(null);
  }

  obterAccessToken(): string | null {
    return localStorage.getItem('meu_token_jwt');
  }

  obterRefreshToken(): string | null {
    return localStorage.getItem('meu_refresh_token');
  }

  renovarToken(): Observable<any> {
    const accessToken = this.obterAccessToken();
    const refreshToken = this.obterRefreshToken();

    if (!accessToken || !refreshToken) {
      this.fazerLogout();
      return throwError(() => 'Tokens não encontrados');
    }

    return this.http.post<any>(`${this.baseUrl}/refresh-token`, { accessToken, refreshToken }).pipe(
      tap((resposta: any) => {
        localStorage.setItem('meu_token_jwt', resposta.accessToken);
        localStorage.setItem('meu_refresh_token', resposta.refreshToken);
      }),
      catchError(erro => {
        this.fazerLogout();
        return throwError(() => erro);
      })
    );
  }

  obterId(){
    const id = localStorage.getItem('meu_app_user_id');
    return id ? Number(id) : 0;
  }
}

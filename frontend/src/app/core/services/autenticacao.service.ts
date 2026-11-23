import { Injectable, signal, computed, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap, catchError, throwError, shareReplay, finalize } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginUsuarioResponse } from '../models/usuario.interface';

@Injectable({
  providedIn: 'root'
})
export class AutenticacaoService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private readonly baseUrl = `${environment.apiUrl}/Usuario`;

  private stringId = localStorage.getItem('meu_app_user_id') ?? null;
  usuarioId = signal<number | null>(this.stringId ? Number(this.stringId) : null);

  logado = computed(() => this.usuarioId() !== null);

  // Evita race condition quando várias requisições disparam refresh ao mesmo tempo
  private refreshEmAndamento$: Observable<LoginUsuarioResponse> | null = null;

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
    this.router.navigate(['/login']);
  }

  obterAccessToken(): string | null {
    return localStorage.getItem('meu_token_jwt');
  }

  obterRefreshToken(): string | null {
    return localStorage.getItem('meu_refresh_token');
  }

  /**
   * Retorna true se o JWT já expirou ou falta menos de `margemMinutos` para expirar.
   */
  tokenProximoDeExpirar(margemMinutos = 120): boolean {
    const token = this.obterAccessToken();
    if (!token) return false;

    try {
      const payloadBase64 = token.split('.')[1];
      if (!payloadBase64) return false;
      const base64 = payloadBase64.replace(/-/g, '+').replace(/_/g, '/');
      const payload = JSON.parse(atob(base64));
      if (!payload.exp) return false;

      const expiraEmMs = payload.exp * 1000;
      const margemMs = margemMinutos * 60 * 1000;
      return Date.now() >= expiraEmMs - margemMs;
    } catch {
      return false;
    }
  }

  renovarToken(): Observable<LoginUsuarioResponse> {
    if (this.refreshEmAndamento$) {
      return this.refreshEmAndamento$;
    }

    const accessToken = this.obterAccessToken();
    const refreshToken = this.obterRefreshToken();

    if (!accessToken || !refreshToken) {
      this.fazerLogout();
      return throwError(() => 'Tokens não encontrados');
    }

    this.refreshEmAndamento$ = this.http
      .post<LoginUsuarioResponse>(`${this.baseUrl}/refresh-token`, { accessToken, refreshToken })
      .pipe(
        tap((resposta: LoginUsuarioResponse) => {
          if (resposta.accessToken && resposta.refreshToken) {
            localStorage.setItem('meu_token_jwt', resposta.accessToken);
            localStorage.setItem('meu_refresh_token', resposta.refreshToken);
          }
        }),
        catchError(erro => {
          this.fazerLogout();
          return throwError(() => erro);
        }),
        finalize(() => {
          this.refreshEmAndamento$ = null;
        }),
        shareReplay(1)
      );

    return this.refreshEmAndamento$;
  }

  obterId() {
    const id = localStorage.getItem('meu_app_user_id');
    return id ? Number(id) : 0;
  }
}

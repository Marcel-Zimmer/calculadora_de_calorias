import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, finalize, switchMap } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { CarragamentoService } from '../services/carregamento.service';
import { AutenticacaoService } from '../services/autenticacao.service';

export const apiInterceptor: HttpInterceptorFn = (req, next) => {
  const carregamentoService = inject(CarragamentoService);
  const authService = inject(AutenticacaoService);

  const ehRotaAuth =
    req.url.includes('refresh-token') ||
    req.url.includes('/login') ||
    req.url.includes('/registrar');

  carregamentoService.mostrar();

  const enviarComToken = (token: string | null) => {
    const requestClonada =
      token && !ehRotaAuth
        ? req.clone({
            setHeaders: {
              Authorization: `Bearer ${token}`
            }
          })
        : req;

    return next(requestClonada);
  };

  const tokenAtual = authService.obterAccessToken();
  const temRefresh = !!authService.obterRefreshToken();

  // Renovação proativa conforme o uso: se faltar < 2h para expirar, renova antes de disparar a requisição
  const fluxoInicial$ =
    !ehRotaAuth && tokenAtual && temRefresh && authService.tokenProximoDeExpirar(120)
      ? authService.renovarToken().pipe(
          switchMap((novaResposta) => enviarComToken(novaResposta.accessToken ?? null))
        )
      : enviarComToken(tokenAtual);

  return fluxoInicial$.pipe(
    catchError((erro: HttpErrorResponse) => {
      // Fallback: se ainda assim retornar 401 (ex: clock skew ou revogação), tenta renovar uma vez
      if (erro.status === 401 && !ehRotaAuth) {
        return authService.renovarToken().pipe(
          switchMap((novaResposta) => enviarComToken(novaResposta.accessToken ?? null)),
          catchError(() => {
            authService.fazerLogout();
            return throwError(() => 'Sua sessão expirou. Faça login novamente.');
          })
        );
      }

      let mensagemErro = 'Ocorreu um erro inesperado.';

      if (erro.status === 0) {
        mensagemErro = 'Não foi possível conectar ao servidor. Verifique sua internet.';
      } else if (erro.status === 400 && erro.error?.errors) {
        const validacoes = erro.error.errors;
        const primeiroErro = Object.values(validacoes)[0] as string[];
        mensagemErro = primeiroErro[0];
      } else if (erro.status === 401) {
        mensagemErro = 'Sua sessão expirou. Faça login novamente.';
      } else if (erro.status === 403) {
        mensagemErro = 'Você não tem permissão para acessar este recurso.';
      } else if (erro.status >= 500) {
        mensagemErro = 'Erro interno do servidor.';
      } else if (erro.error && typeof erro.error === 'string') {
        mensagemErro = erro.error;
      }

      return throwError(() => mensagemErro);
    }),
    finalize(() => {
      carregamentoService.esconder();
    })
  );
};

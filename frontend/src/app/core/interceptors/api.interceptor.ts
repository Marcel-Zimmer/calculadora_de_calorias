import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, finalize, switchMap } from 'rxjs/operators';
import { throwError, of } from 'rxjs';
import { CarragamentoService } from '../services/carregamento.service';
import { AutenticacaoService } from '../services/autenticacao.service'; 

export const apiInterceptor: HttpInterceptorFn = (req, next) =>
{
  const carregamentoService = inject(CarragamentoService);
  const authService = inject(AutenticacaoService);
  
  carregamentoService.mostrar();
  
  const token = authService.obterAccessToken(); 
  let requestClonada = req;

  if (token && !req.url.includes('refresh-token')) {
    requestClonada = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

  return next(requestClonada)
    .pipe(
      catchError((erro: HttpErrorResponse) => {
        // Se for 401 e não for a própria chamada de refresh-token, tenta renovar
        if (erro.status === 401 && !req.url.includes('refresh-token')) {
          return authService.renovarToken().pipe(
            switchMap((novaResposta: any) => {
              // Se renovou com sucesso, repete a requisição original com o novo token
              const novoToken = novaResposta.accessToken;
              const novaRequisicao = req.clone({
                setHeaders: {
                  Authorization: `Bearer ${novoToken}`
                }
              });
              return next(novaRequisicao);
            }),
            catchError((erroRefresh) => {
              // Se a renovação falhar, desloga e manda o erro
              authService.fazerLogout();
              return throwError(() => 'Sua sessão expirou. Faça login novamente.');
            })
          );
        }

        let mensagemErro = 'Ocorreu um erro inesperado.';

        if (erro.status === 0) {
          mensagemErro = 'Não foi possível conectar ao servidor. Verifique sua internet.';
        } 
        else if (erro.status === 400 && erro.error?.errors) { 
          const validacoes = erro.error.errors;
          const primeiroErro = Object.values(validacoes)[0] as string[];
          mensagemErro = primeiroErro[0];
        } 
        else if (erro.status === 401) {
          mensagemErro = 'Sua sessão expirou. Faça login novamente.';
        } else if (erro.status === 403) {
          mensagemErro = 'Você não tem permissão para acessar este recurso.';
        } 
        else if (erro.status >= 500) {
          mensagemErro = 'Erro interno do servidor.';
        }
        else if (erro.error && typeof erro.error === 'string') {
          mensagemErro = erro.error;
        }

        return throwError(() => mensagemErro); 
      }),
      finalize(() => {
        carregamentoService.esconder();
      })
    );
};

import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, delay, finalize } from 'rxjs/operators';
import { throwError } from 'rxjs';
import { CarragamentoService } from '../services/carregamento.service';
import { AutenticacaoService } from '../services/autenticacao.service'; 

export const apiInterceptor: HttpInterceptorFn = (req, next) =>
{
  const carregamentoService = inject(CarragamentoService);
  carregamentoService.mostrar();
  
  const token = localStorage.getItem('meu_token_jwt'); 
  let requestClonada = req;

  if (token) {
    requestClonada = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }

return next(requestClonada)
  .pipe(
    catchError((erro: HttpErrorResponse) => {
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
        mensagemErro = 'Backend quebrou.';
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
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

  return next(requestClonada).pipe(
    catchError((erro: HttpErrorResponse) => {
      if (erro.status === 401) {
        console.error('Não autorizado! O token expirou ou é inválido.');
      } else if (erro.status === 403) {
        console.error('Sem permissão para acessar este recurso.');
      } else if (erro.status >= 500) {
        console.error('Erro interno no servidor (.NET chorou).');
      }

      return throwError(() => erro); 
    }),

    finalize(() => {
      carregamentoService.esconder();
    })
  );
};
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AutenticacaoService } from '../services/autenticacao.service'; 

export const autenticadoGuard: CanActivateFn = (route, state) => {
  const autenticacao = inject(AutenticacaoService);
  const router = inject(Router);

  if (autenticacao.logado()) {
    return true;
  }

  router.navigate(['/login']);
  return false;
};

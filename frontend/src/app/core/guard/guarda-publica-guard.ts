import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AutenticacaoService } from '../services/autenticacao.service'; 

export const guardaPublicaGuard: CanActivateFn = (route, state) => {
  const autenticacao = inject(AutenticacaoService);
  const router = inject(Router);

  if (autenticacao.logado()) {
    router.navigate(['/dashboard']);
    return false; 
  }//else{
  //   router.navigate(['/login']);
  //   return true;     
  // }
  return true
};

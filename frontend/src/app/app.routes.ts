import { Routes } from '@angular/router';
import { Dashboard } from './features/dashboard/dashboard';
import { Login } from './features/login/login';
import { Cadastro } from './features/cadastro/cadastro';
import { guardaPublicaGuard } from './core/guard/guarda-publica-guard';
import { autenticadoGuard } from './core/guard/autenticado-guard';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: Dashboard, canActivate: [autenticadoGuard] }, 
  { path: 'login', component: Login, canActivate: [guardaPublicaGuard] },
  { path: 'cadastro', component: Cadastro, canActivate: [guardaPublicaGuard] },
  { path: '**', redirectTo: 'dashboard' }
];

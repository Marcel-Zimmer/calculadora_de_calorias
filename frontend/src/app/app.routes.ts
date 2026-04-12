import { Routes } from '@angular/router';
import { Dashboard } from './features/dashboard/dashboard';
import { Login } from './features/login/login';
import { Cadastro } from './features/cadastro/cadastro';
import { guardaPublicaGuard } from './core/guard/guarda-publica-guard';

export const routes: Routes = [
  { path: 'dashboard', component: Dashboard}, 
  { path: 'login', component:Login ,},
  { path: 'cadastro', component: Cadastro }
];

import { Routes } from '@angular/router';
import { authGuard } from './guards/auth-guard';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'login', loadComponent: () => import('./pages/login/login').then(m => m.Login) },
  { path: 'dashboard', loadComponent: () => import('./pages/dashboard/dashboard').then(m => m.Dashboard), canActivate: [authGuard] },
  { path: 'produtos', loadComponent: () => import('./pages/produtos/produtos').then(m => m.Produtos), canActivate: [authGuard] },
  { path: 'estoque', loadComponent: () => import('./pages/estoque/estoque').then(m => m.Estoque), canActivate: [authGuard] },
  { path: '**', redirectTo: 'dashboard' }
];
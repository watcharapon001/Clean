import { Routes } from '@angular/router';

export const SU_ROUTES: Routes = [
  {
    path: 'surt01',
    loadComponent: () => import('./surt01/surt01.component').then(m => m.Surt01Component),
    data: { programCode: 'SURT01' }
  },
  {
    path: 'surt01/detail/new',
    loadComponent: () => import('./surt01/surt01-detail.component').then(m => m.Surt01DetailComponent),
    data: { programCode: 'SURT01' }
  },
  {
    path: 'surt01/detail/:id',
    loadComponent: () => import('./surt01/surt01-detail.component').then(m => m.Surt01DetailComponent),
    data: { programCode: 'SURT01' }
  },
  {
    path: 'surt02',
    loadComponent: () => import('./surt02/surt02.component').then(m => m.Surt02Component),
    data: { programCode: 'SURT02' }
  },
  {
    path: 'surt02/detail/new',
    loadComponent: () => import('./surt02/surt02-detail.component').then(m => m.Surt02DetailComponent),
    data: { programCode: 'SURT02' }
  },
  {
    path: 'surt02/detail/:id',
    loadComponent: () => import('./surt02/surt02-detail.component').then(m => m.Surt02DetailComponent),
    data: { programCode: 'SURT02' }
  },
  {
    path: 'surt03',
    loadComponent: () => import('./surt03/surt03.component').then(m => m.Surt03Component),
    data: { programCode: 'SURT03' }
  },
  {
    path: 'surt04',
    loadComponent: () => import('./surt04/surt04.component').then(m => m.Surt04Component),
    data: { programCode: 'SURT04' }
  },
  {
    path: 'surt04/detail/new',
    loadComponent: () => import('./surt04/surt04-detail.component').then(m => m.Surt04DetailComponent),
    data: { programCode: 'SURT04' }
  },
  {
    path: 'surt04/detail/:id',
    loadComponent: () => import('./surt04/surt04-detail.component').then(m => m.Surt04DetailComponent),
    data: { programCode: 'SURT04' }
  }
];

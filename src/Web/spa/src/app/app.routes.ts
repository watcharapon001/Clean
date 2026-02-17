import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { LoginComponent } from './features/auth/login/login.component';
import { SwitchOrgComponent } from './features/auth/switch-org/switch-org.component';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';

export const routes: Routes = [
  {
    path: 'auth/login',
    component: LoginComponent
  },
  {
    path: '',
    component: MainLayoutComponent,
    canActivate: [authGuard],
    children: [
      { 
        path: 'dashboard', 
        loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent),
        data: { programCode: 'DBRT00' }
      },
      
      // DB Module (DBRT01)
      { 
        path: 'db/dbrt01', 
        loadComponent: () => import('./features/db/dbrt01/dbrt01.component').then(m => m.Dbrt01Component),
        data: { programCode: 'DBRT01' }
      },
      { 
        path: 'db/dbrt01/detail/new', 
        loadComponent: () => import('./features/db/dbrt01/dbrt01-detail.component').then(m => m.Dbrt01DetailComponent),
        data: { programCode: 'DBRT01' }
      },
      { 
        path: 'db/dbrt01/detail/:id', 
        loadComponent: () => import('./features/db/dbrt01/dbrt01-detail.component').then(m => m.Dbrt01DetailComponent),
        data: { programCode: 'DBRT01' }
      },

      // SU Module (System Setup)
      {
        path: 'su',
        loadChildren: () => import('./features/su/su.routes').then(m => m.SU_ROUTES)
      }
    ]
  },
  {
    path: 'auth/switch-org',
    component: SwitchOrgComponent
  },
  {
    path: '**',
    redirectTo: ''
  }
];

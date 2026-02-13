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
        path: '',
        component: DashboardComponent // Replaced SwitchOrgComponent
      },
      {
        path: 'auth/switch-org',
        component: SwitchOrgComponent
      }
    ]
  },
  {
    path: '**',
    redirectTo: ''
  }
];

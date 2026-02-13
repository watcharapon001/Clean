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
      { path: 'dashboard', loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent) },
      
      // DB Module
      { path: 'db/employees', loadComponent: () => import('./features/db/employee-list/employee-list.component').then(m => m.EmployeeListComponent) },
      { path: 'db/employees/new', loadComponent: () => import('./features/db/employee-form/employee-form.component').then(m => m.EmployeeFormComponent) },
      { path: 'db/employees/:id', loadComponent: () => import('./features/db/employee-form/employee-form.component').then(m => m.EmployeeFormComponent) },
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

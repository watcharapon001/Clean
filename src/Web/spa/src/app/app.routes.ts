import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { LoginComponent } from './features/auth/login/login.component'; // Lazy loading is better for features, but direct import for now is fine for small apps
import { SwitchOrgComponent } from './features/auth/switch-org/switch-org.component';

export const routes: Routes = [
  {
    path: 'auth/login',
    component: LoginComponent
  },
  {
    path: 'auth/switch-org',
    component: SwitchOrgComponent,
    canActivate: [authGuard]
  },
  {
    path: '',
    canActivate: [authGuard], 
    component: SwitchOrgComponent // Temporary PLACEHOLDER: Redirect to dashboard or home usually. 
    // For now, let's just show SwitchOrg or a dummy home if verified. 
    // Ideally we have a storage for 'Dashboard' or 'Home'.
    // Let's create a simple inline component for Home to verify auth works.
  },
  {
    path: '**',
    redirectTo: ''
  }
];

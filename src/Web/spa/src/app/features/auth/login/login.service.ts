import { Injectable, inject } from '@angular/core';
import { AuthService, LoginResponse } from '../../../core/auth/auth.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root' // Or providedIn: 'any' if we want it isolated, but root is standard for services unless we have a specific module file
})
export class LoginService {
  private authService = inject(AuthService);

  login(credentials: { username: string; password: string }): Observable<LoginResponse> {
    return this.authService.login(credentials);
  }
}

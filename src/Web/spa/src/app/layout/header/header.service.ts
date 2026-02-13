import { Injectable, inject, Signal } from '@angular/core';
import { AuthService, Organization, User, LoginResponse } from '../../core/auth/auth.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HeaderService {
  private authService = inject(AuthService);

  get currentOrg(): Signal<Organization | null> {
    return this.authService.currentOrg;
  }

  get currentUser(): Signal<User | null> {
    return this.authService.currentUser;
  }

  getOrgs(): Observable<Organization[]> {
    return this.authService.getOrgs();
  }

  switchOrg(orgId: string): Observable<LoginResponse> {
    return this.authService.switchOrg(orgId);
  }

  logout(): void {
    this.authService.logout();
  }
}

import { Injectable, inject, Signal } from '@angular/core';
import { AuthService, Organization, LoginResponse } from '../../../core/auth/auth.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SwitchOrgService {
  private authService = inject(AuthService);

  get currentOrg(): Signal<Organization | null> {
    return this.authService.currentOrg;
  }

  getOrgs(): Observable<Organization[]> {
    return this.authService.getOrgs();
  }

  switchOrg(orgId: string): Observable<LoginResponse> {
    return this.authService.switchOrg(orgId);
  }
}

import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { catchError, map, Observable, of, tap, throwError } from 'rxjs';
import { TokenService } from './token.service';

export interface User {
  userId: string;
  username: string;
  firstName?: string;
  lastName?: string;
  email?: string;
}

export interface Organization {
  orgId: string;
  orgCode: string;
  orgName: string;
  isDefault: boolean;
}

export interface LoginResponse {
  accessToken: string;
  expiresInSeconds: number;
  orgs: Organization[];
  defaultOrgId: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly keyCurrentUser = 'current-user';
  private readonly keyCurrentOrg = 'current-org';

  // State Signals
  private _currentUser = signal<User | null>(null);
  private _currentOrg = signal<Organization | null>(null);

  // Computed signals
  currentUser = computed(() => this._currentUser());
  currentOrg = computed(() => this._currentOrg());
  isAuthenticated = computed(() => !!this._currentUser());

  constructor(
    private http: HttpClient,
    private tokenService: TokenService,
    private router: Router
  ) {
    // Attempt to restore state from storage on init
    this.restoreState();
  }

  // API Methods
  login(credentials: { username: string; password: string }): Observable<LoginResponse> {
    return this.http.post<LoginResponse>('/api/auth/login', credentials).pipe(
      tap(response => this.handleAuthResponse(response))
    );
  }

  getMe(): Observable<User> {
    return this.http.get<User>('/api/auth/me').pipe(
      tap(user => {
        this._currentUser.set(user);
        this.saveState();
      })
    );
  }

  getOrgs(): Observable<Organization[]> {
    return this.http.get<Organization[]>('/api/auth/orgs');
  }

  switchOrg(orgId: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>('/api/auth/switch-org', { orgId }).pipe(
      tap(response => this.handleAuthResponse(response))
    );
  }

  logout(): void {
    this.tokenService.clearToken();
    this._currentUser.set(null);
    this._currentOrg.set(null);
    this.clearState();
    this.router.navigate(['/auth/login']);
  }

  // Session Restoration Logic
  // Should serve as APP_INITIALIZER
  initialize(): Observable<any> {
    const token = this.tokenService.getToken();
    if (!token) {
      return of(null);
    }

    // Parallel fetch: validation happens via these calls
    // If 401, Interceptor will catch it.
    // We try to verify token by fetching Me usually.
    return this.getMe().pipe(
      catchError(() => {
        // Token invalid or network error
        // interceptor might handle redirect, but safe fallback:
        // this.logout(); // handled by interceptor usually
        return of(null);
      })
    );
  }

  private handleAuthResponse(response: LoginResponse): void {
    this.tokenService.setToken(response.accessToken);
    
    // Set Current Org
    const org = response.orgs.find(o => o.orgId === response.defaultOrgId);
    if (org) {
      this._currentOrg.set(org);
    }

    // We might need to fetch User details separately if not in login response, 
    // or decode token. For now, we trust getMe() to follow up or decode here if needed.
    // Ideally LoginResponse might contain User info or we fetch it.
    // Let's assume we fetch `getMe()` immediately after or user needs to call it.
    // Updated plan: fetch Me immediately to populate state.
    this.getMe().subscribe(); 
  }

  private saveState(): void {
    if (this._currentUser()) localStorage.setItem(this.keyCurrentUser, JSON.stringify(this._currentUser()));
    if (this._currentOrg()) localStorage.setItem(this.keyCurrentOrg, JSON.stringify(this._currentOrg()));
  }

  private restoreState(): void {
    const user = localStorage.getItem(this.keyCurrentUser);
    const org = localStorage.getItem(this.keyCurrentOrg);

    if (user) this._currentUser.set(JSON.parse(user));
    if (org) this._currentOrg.set(JSON.parse(org));
  }

  private clearState(): void {
    localStorage.removeItem(this.keyCurrentUser);
    localStorage.removeItem(this.keyCurrentOrg);
  }
}

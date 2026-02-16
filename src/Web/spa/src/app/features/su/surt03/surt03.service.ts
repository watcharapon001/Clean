import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MenuPermission } from './surt03.component';
import { Profile } from '../surt01/surt01.component';

@Injectable({
  providedIn: 'root'
})
export class Surt03Service {
  private http = inject(HttpClient);
  private apiUrl = '/api/su/surt03';

  getProfiles(): Observable<Profile[]> {
    return this.http.get<Profile[]>('/api/su/surt01');
  }

  getPermissions(profileId: string): Observable<MenuPermission[]> {
    return this.http.get<MenuPermission[]>(`${this.apiUrl}/${profileId}`);
  }

  updatePermissions(profileId: string, permissions: any[]): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${profileId}`, { 
      profileId, 
      permissions 
    });
  }
}

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Profile } from './surt01.component';

@Injectable({
  providedIn: 'root'
})
export class Surt01Service {
  private http = inject(HttpClient);
  private apiUrl = '/api/su/surt01';

  getProfiles(): Observable<Profile[]> {
    return this.http.get<Profile[]>(this.apiUrl);
  }

  getProfile(id: string): Observable<Profile> {
    return this.http.get<Profile>(`${this.apiUrl}/${id}`);
  }

  createProfile(profile: Profile): Observable<string> {
    return this.http.post<string>(this.apiUrl, profile);
  }

  updateProfile(id: string, profile: Profile): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, profile);
  }

  deleteProfile(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Profile } from './surt01.component';

export interface PaginatedList<T> {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class Surt01Service {
  private http = inject(HttpClient);
  private apiUrl = '/api/su/surt01';

  getProfiles(
    pageNumber: number = 1,
    pageSize: number = 10,
    sortColumn?: string,
    sortDirection?: string
  ): Observable<PaginatedList<Profile>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (sortColumn) {
      params = params.set('sortColumn', sortColumn);
      if (sortDirection) {
        params = params.set('sortDirection', sortDirection);
      }
    }

    return this.http.get<PaginatedList<Profile>>(this.apiUrl, { params });
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

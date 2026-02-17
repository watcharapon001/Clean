import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Dbrt01 {
  employeeId: string;
  orgId: string;
  employeeCode: string;
  firstName?: string;
  lastName?: string;
  displayName?: string;
  email?: string;
  phone?: string;
  isActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class Dbrt01Service {
  private http = inject(HttpClient);
  private apiUrl = '/api/dbrt01';

  getDbrt01s(): Observable<Dbrt01[]> {
    return this.http.get<Dbrt01[]>(this.apiUrl);
  }

  getDbrt01(id: string): Observable<Dbrt01> {
    return this.http.get<Dbrt01>(`${this.apiUrl}/${id}`);
  }

  createDbrt01(employee: Partial<Dbrt01>): Observable<string> {
    return this.http.post(this.apiUrl, employee, { responseType: 'text' });
  }

  updateDbrt01(id: string, employee: Partial<Dbrt01>): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, employee);
  }

  deleteDbrt01(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

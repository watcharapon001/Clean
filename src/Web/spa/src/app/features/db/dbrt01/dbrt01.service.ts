import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedList } from '../../su/surt01/surt01.service';

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

  getDbrt01s(
    pageNumber: number = 1,
    pageSize: number = 10,
    sortColumn?: string,
    sortDirection?: string
  ): Observable<PaginatedList<Dbrt01>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (sortColumn) {
      params = params.set('sortColumn', sortColumn);
      if (sortDirection) {
        params = params.set('sortDirection', sortDirection);
      }
    }

    return this.http.get<PaginatedList<Dbrt01>>(this.apiUrl, { params });
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

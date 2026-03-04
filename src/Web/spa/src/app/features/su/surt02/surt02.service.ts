import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Menu } from './surt02.component';
import { PaginatedList } from '../surt01/surt01.service'; // Reuse interface

@Injectable({
  providedIn: 'root'
})
export class Surt02Service {
  private http = inject(HttpClient);
  private apiUrl = '/api/su/surt02';

  getMenus(
    pageNumber: number = 1,
    pageSize: number = 10,
    sortColumn?: string,
    sortDirection?: string
  ): Observable<PaginatedList<Menu>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (sortColumn) {
      params = params.set('sortColumn', sortColumn);
      if (sortDirection) {
        params = params.set('sortDirection', sortDirection);
      }
    }

    return this.http.get<PaginatedList<Menu>>(this.apiUrl, { params });
  }

  getMenu(id: string): Observable<Menu> {
    return this.http.get<Menu>(`${this.apiUrl}/${id}`);
  }

  createMenu(menu: Menu): Observable<string> {
    return this.http.post<string>(this.apiUrl, menu);
  }

  updateMenu(id: string, menu: Menu): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, menu);
  }

  deleteMenu(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Menu } from './surt02.component';

@Injectable({
  providedIn: 'root'
})
export class Surt02Service {
  private http = inject(HttpClient);
  private apiUrl = '/api/su/surt02';

  getMenus(): Observable<Menu[]> {
    return this.http.get<Menu[]>(this.apiUrl);
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

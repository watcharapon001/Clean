import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Menu } from './menu.model';

@Injectable({
  providedIn: 'root'
})
export class SidebarService {
  private http = inject(HttpClient);

  getMenus(): Observable<Menu[]> {
    return this.http.get<Menu[]>('/api/menus/current-user');
  }
}

import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedList } from '../surt01/surt01.service';

export interface Organize {
  orgId: string;
  orgCode: string;
  orgName: string;
  isActive: boolean;
}

export interface CreateOrganizeCommand {
  orgCode: string;
  orgName: string;
  isActive: boolean;
}

export interface UpdateOrganizeCommand {
  orgId: string;
  orgName: string;
  isActive: boolean;
}

@Injectable({ providedIn: 'root' })
export class Surt06Service {
  private http = inject(HttpClient);
  private apiUrl = '/api/su/surt06';

  getOrganizes(
    pageNumber: number = 1,
    pageSize: number = 10,
    sortColumn?: string,
    sortDirection?: string
  ): Observable<PaginatedList<Organize>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (sortColumn) {
      params = params.set('sortColumn', sortColumn);
      if (sortDirection) {
        params = params.set('sortDirection', sortDirection);
      }
    }

    return this.http.get<PaginatedList<Organize>>(`${this.apiUrl}/list`, { params });
  }

  createOrganize(command: CreateOrganizeCommand): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/create`, command);
  }

  updateOrganize(command: UpdateOrganizeCommand): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/update`, command);
  }

  deleteOrganize(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/delete/${id}`);
  }
}

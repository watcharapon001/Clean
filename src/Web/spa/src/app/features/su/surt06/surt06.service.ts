import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

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

  getOrganizes(): Observable<Organize[]> {
    return this.http.get<Organize[]>(`${this.apiUrl}/list`);
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

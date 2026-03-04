import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedList } from '../surt01/surt01.service';

export interface SuConfig {
  configKey: string;
  configValue: string;
  description?: string;
  dataType: string;
}

export interface UpdateConfigCommand {
  configKey: string;
  configValue: string;
}

@Injectable({ providedIn: 'root' })
export class Surt07Service {
  private http = inject(HttpClient);
  private apiUrl = '/api/su/surt07';

  getConfigs(
    pageNumber: number = 1,
    pageSize: number = 10,
    sortColumn?: string,
    sortDirection?: string
  ): Observable<PaginatedList<SuConfig>> {
    let params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    if (sortColumn) {
      params = params.set('sortColumn', sortColumn);
      if (sortDirection) {
        params = params.set('sortDirection', sortDirection);
      }
    }

    return this.http.get<PaginatedList<SuConfig>>(`${this.apiUrl}/list`, { params });
  }

  updateConfig(command: UpdateConfigCommand): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/update`, command);
  }
}

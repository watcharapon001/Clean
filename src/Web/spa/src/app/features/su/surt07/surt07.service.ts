import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

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

  getConfigs(): Observable<SuConfig[]> {
    return this.http.get<SuConfig[]>(`${this.apiUrl}/list`);
  }

  updateConfig(command: UpdateConfigCommand): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/update`, command);
  }
}

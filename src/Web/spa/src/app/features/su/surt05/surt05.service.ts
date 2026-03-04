import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface AuditLog {
  auditLogId: string;
  userId?: string;
  action: string;
  tableName: string;
  keyValues?: string;
  oldValues?: string;
  newValues?: string;
  timestamp: string;
}

@Injectable({ providedIn: 'root' })
export class Surt05Service {
  private http = inject(HttpClient);
  private apiUrl = '/api/su/surt05';

  getAuditLogs(): Observable<AuditLog[]> {
    return this.http.get<AuditLog[]>(`${this.apiUrl}/audit-logs`);
  }

  exportAuditLogs(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/export`, {
      responseType: 'blob'
    });
  }
}

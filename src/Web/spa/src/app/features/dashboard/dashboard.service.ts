import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface RecentAudit {
  action: string;
  tableName: string;
  timestamp: string;
}

export interface DashboardMetrics {
  totalUsers: number;
  totalOrganizes: number;
  totalProfiles: number;
  totalMenus: number;
  recentAudits: RecentAudit[];
}

@Injectable({ providedIn: 'root' })
export class DashboardService {
  private http = inject(HttpClient);
  private apiUrl = '/api/su/dash01';

  getMetrics(): Observable<DashboardMetrics> {
    return this.http.get<DashboardMetrics>(`${this.apiUrl}/metrics`);
  }
}

import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { DashboardService, DashboardMetrics, RecentAudit } from './dashboard.service';
import { AppCardComponent } from '../../shared/components/card/card.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule, 
    RouterModule, 
    MatCardModule, 
    MatIconModule, 
    MatButtonModule,
    MatTableModule,
    AppCardComponent
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  private dashboardService = inject(DashboardService);

  totalUsers = signal(0);
  totalOrganizes = signal(0);
  totalProfiles = signal(0);
  totalMenus = signal(0);
  recentAudits = signal<RecentAudit[]>([]);

  displayedColumns: string[] = ['timestamp', 'action', 'tableName'];

  ngOnInit() {
    this.loadDashboardData();
  }

  loadDashboardData() {
    this.dashboardService.getMetrics().subscribe(metrics => {
      this.totalUsers.set(metrics.totalUsers);
      this.totalOrganizes.set(metrics.totalOrganizes);
      this.totalProfiles.set(metrics.totalProfiles);
      this.totalMenus.set(metrics.totalMenus);
      this.recentAudits.set(metrics.recentAudits);
    });
  }
}

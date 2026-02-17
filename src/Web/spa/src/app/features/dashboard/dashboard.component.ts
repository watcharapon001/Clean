import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { Dbrt01Service, Dbrt01 } from '../db/dbrt01/dbrt01.service';
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
  private dbrt01Service = inject(Dbrt01Service);

  totalEmployees = signal(0);
  activeEmployees = signal(0);
  inactiveEmployees = signal(0);
  recentEmployees = signal<Dbrt01[]>([]);

  displayedColumns: string[] = ['name', 'department', 'status']; // Simplified columns for dashboard

  ngOnInit() {
    this.loadDashboardData();
  }

  loadDashboardData() {
    this.dbrt01Service.getDbrt01s().subscribe(employees => {
      this.totalEmployees.set(employees.length);
      this.activeEmployees.set(employees.filter(e => e.isActive).length);
      this.inactiveEmployees.set(employees.length - this.activeEmployees());
      
      // Get last 5 employees (assuming API returns in some order, or we slice the end)
      // For now, let's just take the last 5 if the array is populated
      this.recentEmployees.set(employees.slice(-5).reverse());
    });
  }
}

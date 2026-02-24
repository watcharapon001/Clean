import { Component, inject, signal, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { Dbrt01Service, Dbrt01 } from './dbrt01.service';
import { AppCardComponent } from '../../../shared/components/card/card.component';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import { ActionBarComponent } from '../../../shared/components/action-bar/action-bar.component';

@Component({
  selector: 'app-dbrt01',
  standalone: true,
  imports: [
    CommonModule, 
    RouterModule, 
    MatTableModule, 
    MatPaginatorModule, 
    MatSortModule, 
    MatButtonModule, 
    MatIconModule,
    MatChipsModule,
    MatDialogModule,
    AppCardComponent,
    ActionBarComponent
  ],
  templateUrl: './dbrt01.component.html',
  styleUrls: ['./dbrt01.component.scss']
})
export class Dbrt01Component implements OnInit {
  private dbrt01Service = inject(Dbrt01Service);
  private dialog = inject(MatDialog);
  
  displayedColumns: string[] = ['index', 'employee', 'contact', 'status', 'actions'];
  dataSource = new MatTableDataSource<Dbrt01>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit() {
    this.loadEmployees();
  }

  loadEmployees() {
    this.dbrt01Service.getDbrt01s().subscribe(data => {
      this.dataSource.data = data;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  deleteEmployee(employee: Dbrt01) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Confirm Delete',
        message: `Are you sure you want to delete <strong>${employee.firstName} ${employee.lastName}</strong>?<br>This action cannot be undone.`
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.dbrt01Service.deleteDbrt01(employee.employeeId).subscribe(() => {
          this.loadEmployees();
        });
      }
    });
  }
}

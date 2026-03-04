import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AppCardComponent } from '../../../shared/components/card/card.component';
import { ActionBarComponent } from '../../../shared/components/action-bar/action-bar.component';
import { Surt05Service, AuditLog } from './surt05.service';
import { JsonDialogComponent } from './json-dialog.component';

@Component({
  selector: 'app-surt05',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    AppCardComponent,
    ActionBarComponent,
    DatePipe
  ],
  templateUrl: './surt05.component.html',
  styleUrl: './surt05.component.scss'
})
export class Surt05Component implements OnInit {
  private service = inject(Surt05Service);
  private dialog = inject(MatDialog);

  displayedColumns: string[] = ['timestamp', 'action', 'tableName', 'userId', 'details'];
  dataSource = new MatTableDataSource<AuditLog>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit() {
    this.loadLogs();
  }

  loadLogs() {
    this.service.getAuditLogs().subscribe(data => {
      this.dataSource.data = data;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  viewDetails(log: AuditLog) {
    this.dialog.open(JsonDialogComponent, {
      width: '600px',
      data: log
    });
  }

  exportData() {
    this.service.exportAuditLogs().subscribe((blob: Blob) => {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = `AuditLogs_${new Date().getTime()}.xlsx`;
      document.body.appendChild(a);
      a.click();
      window.URL.revokeObjectURL(url);
      a.remove();
    });
  }
}

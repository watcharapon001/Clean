import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatTooltipModule } from '@angular/material/tooltip';
import { AppCardComponent } from '../../../shared/components/card/card.component';
import { Surt06Service, Organize } from './surt06.service';
import { Surt06DetailComponent } from './surt06-detail.component';

@Component({
  selector: 'app-surt06',
  standalone: true,
  imports: [
    CommonModule, 
    MatTableModule, 
    MatPaginatorModule, 
    MatSortModule, 
    MatButtonModule, 
    MatIconModule,
    MatDialogModule,
    MatSnackBarModule,
    MatTooltipModule,
    AppCardComponent
  ],
  templateUrl: './surt06.component.html',
  styleUrls: ['./surt06.component.scss']
})
export class Surt06Component implements OnInit {
  private surt06Service = inject(Surt06Service);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  displayedColumns: string[] = ['orgCode', 'orgName', 'status', 'actions'];
  dataSource = new MatTableDataSource<Organize>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.surt06Service.getOrganizes().subscribe(data => {
      this.dataSource.data = data;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  openDialog(organize?: Organize) {
    const dialogRef = this.dialog.open(Surt06DetailComponent, {
      width: '500px',
      data: organize || null
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.loadData();
      }
    });
  }

  deleteOrg(org: Organize) {
    if (confirm(`Are you sure you want to delete ${org.orgName}?`)) {
      this.surt06Service.deleteOrganize(org.orgId).subscribe({
        next: () => {
          this.snackBar.open('Organization deleted successfully', 'Close', { duration: 3000 });
          this.loadData();
        },
        error: (err) => {
          this.snackBar.open(err.error?.detail || err.error?.title || 'Error deleting organization. It might be in use.', 'Close', { duration: 5000 });
        }
      });
    }
  }
}

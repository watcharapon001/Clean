import { Component, inject, OnInit, AfterViewInit, ViewChild } from '@angular/core';
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
import { ActionBarComponent } from '../../../shared/components/action-bar/action-bar.component';
import { Surt06Service, Organize } from './surt06.service';
import { Surt06DetailComponent } from './surt06-detail.component';
import { merge } from 'rxjs';
import { startWith, switchMap, map } from 'rxjs/operators';

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
    AppCardComponent,
    ActionBarComponent
  ],
  templateUrl: './surt06.component.html',
  styleUrls: ['./surt06.component.scss']
})
export class Surt06Component implements OnInit, AfterViewInit {
  private surt06Service = inject(Surt06Service);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  displayedColumns: string[] = ['orgCode', 'status', 'actions'];
  dataSource = new MatTableDataSource<Organize>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  totalCount = 0;

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));

    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        startWith({}),
        switchMap(() => {
          return this.surt06Service.getOrganizes(
            this.paginator.pageIndex + 1,
            this.paginator.pageSize,
            this.sort.active,
            this.sort.direction
          );
        }),
        map((response: any) => {
          this.totalCount = response.totalCount;
          return response.items;
        })
      )
      .subscribe((data: Organize[]) => {
        this.dataSource.data = data;
      });
  }

  loadData() {
    this.paginator.page.emit();
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

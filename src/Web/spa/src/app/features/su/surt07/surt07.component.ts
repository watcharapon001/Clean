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
import { Surt07Service, SuConfig } from './surt07.service';
import { Surt07DetailComponent } from './surt07-detail.component';
import { merge } from 'rxjs';
import { startWith, switchMap, map } from 'rxjs/operators';

@Component({
  selector: 'app-surt07',
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
  templateUrl: './surt07.component.html',
  styleUrls: ['./surt07.component.scss']
})
export class Surt07Component implements OnInit, AfterViewInit {
  private surt07Service = inject(Surt07Service);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  displayedColumns: string[] = ['configKey', 'configValue', 'actions'];
  dataSource = new MatTableDataSource<SuConfig>([]);

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
          return this.surt07Service.getConfigs(
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
      .subscribe((data: SuConfig[]) => {
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

  openDialog(config: SuConfig) {
    const dialogRef = this.dialog.open(Surt07DetailComponent, {
      width: '500px',
      data: config
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.loadData();
      }
    });
  }

}

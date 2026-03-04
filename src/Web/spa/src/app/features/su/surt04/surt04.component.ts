import { Component, inject, OnInit, AfterViewInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AppCardComponent } from '../../../shared/components/card/card.component';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import { ActionBarComponent } from '../../../shared/components/action-bar/action-bar.component';
import { Surt04Service, User } from './surt04.service';
import { merge } from 'rxjs';
import { startWith, switchMap, map } from 'rxjs/operators';

@Component({
  selector: 'app-surt04',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    AppCardComponent,
    ActionBarComponent
  ],
  templateUrl: './surt04.component.html',
  styleUrl: './surt04.component.scss'
})
export class Surt04Component implements OnInit, AfterViewInit {
  private service = inject(Surt04Service);
  private dialog = inject(MatDialog);

  displayedColumns: string[] = ['index', 'username', 'status', 'actions'];
  dataSource = new MatTableDataSource<User>([]);

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
          return this.service.getUsers(
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
      .subscribe((data: User[]) => {
        this.dataSource.data = data;
      });
  }

  loadUsers() {
    this.paginator.page.emit();
  }

  deleteUser(user: User) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Confirm Delete',
        message: `Are you sure you want to delete user <strong>${user.username}</strong>?`
      }
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.service.deleteUser(user.userId).subscribe(() => {
          this.loadUsers();
        });
      }
    });
  }

}

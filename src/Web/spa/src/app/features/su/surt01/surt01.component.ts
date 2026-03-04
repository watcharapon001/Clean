import { Component, inject, signal, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AppCardComponent } from '../../../shared/components/card/card.component';
import { ConfirmDialogComponent } from '../../../shared/components/confirm-dialog/confirm-dialog.component';
import { ActionBarComponent } from '../../../shared/components/action-bar/action-bar.component';
import { Surt01Service } from './surt01.service';
import { merge } from 'rxjs';
import { startWith, switchMap, map } from 'rxjs/operators';

export interface Profile {
  profileId: string;
  profileCode: string;
  profileName: string;
  description?: string;
  isActive: boolean;
}

@Component({
  selector: 'app-surt01',
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
  templateUrl: './surt01.component.html',
  styleUrl: './surt01.component.scss',
})
export class Surt01Component implements OnInit {
  private service = inject(Surt01Service);
  private dialog = inject(MatDialog);

  displayedColumns: string[] = [
    'index',
    'profileCode',
    'description',
    'status',
    'actions',
  ];
  dataSource = new MatTableDataSource<Profile>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  totalCount = 0;

  ngOnInit(): void {
    // Initial load will be triggered by ngAfterViewInit via paginator/sort merge pipeline
  }

  ngAfterViewInit(): void {
    this.sort.sortChange.subscribe(() => (this.paginator.pageIndex = 0));

    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        startWith({}),
        switchMap(() => {
          return this.service.getProfiles(
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
      .subscribe((data: Profile[]) => {
        this.dataSource.data = data;
      });
  }

  loadProfiles() {
    this.paginator.page.emit(); // Trigger the merge pipeline to reload from API
  }

  deleteProfile(profile: Profile) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Confirm Delete',
        message: `Are you sure you want to delete <strong>${profile.profileName}</strong>?`,
      },
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.service.deleteProfile(profile.profileId).subscribe(() => {
          this.loadProfiles();
        });
      }
    });
  }

}

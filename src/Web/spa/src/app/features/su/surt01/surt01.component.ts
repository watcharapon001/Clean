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
    'profileName',
    'description',
    'status',
    'actions',
  ];
  dataSource = new MatTableDataSource<Profile>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit() {
    this.loadProfiles();
  }

  loadProfiles() {
    this.service.getProfiles().subscribe((data) => {
      this.dataSource.data = data;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
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

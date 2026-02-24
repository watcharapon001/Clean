import { Component, inject, signal, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
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
import { Surt02Service } from './surt02.service';

export interface Menu {
  menuId: string;
  menuCode: string;
  menuName: string;
  route?: string;
  icon?: string;
  sequence: number;
  parentMenuId?: string;
  parentMenuName?: string;
  isActive: boolean;
}

@Component({
  selector: 'app-surt02',
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
  templateUrl: './surt02.component.html',
  styleUrl: './surt02.component.scss'
})
export class Surt02Component implements OnInit {
  private service = inject(Surt02Service);
  private dialog = inject(MatDialog);
  
  displayedColumns: string[] = ['index', 'menuCode', 'menuName', 'parentMenu', 'route', 'sequence', 'status', 'actions'];
  dataSource = new MatTableDataSource<Menu>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  ngOnInit() {
    this.loadMenus();
  }

  loadMenus() {
    this.service.getMenus().subscribe(data => {
      this.dataSource.data = data;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
    });
  }

  deleteMenu(menu: Menu) {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '400px',
      data: {
        title: 'Confirm Delete',
        message: `Are you sure you want to delete <strong>${menu.menuName}</strong>?`
      }
    });

    dialogRef.afterClosed().subscribe((result: any) => {
      if (result) {
        this.service.deleteMenu(menu.menuId).subscribe(() => {
          this.loadMenus();
        });
      }
    });
  }
}

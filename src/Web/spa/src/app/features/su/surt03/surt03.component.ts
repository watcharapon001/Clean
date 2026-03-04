import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AppCardComponent } from '../../../shared/components/card/card.component';
import { ActionBarComponent } from '../../../shared/components/action-bar/action-bar.component';
import { SelectInputComponent } from '../../../shared/components/inputs';
import { Profile } from '../surt01/surt01.component';
import { Surt03Service } from './surt03.service';

export interface MenuPermission {
  menuId: string;
  menuCode: string;
  menuName: string;
  parentMenuId?: string;
  parentMenuName?: string;
  sequence: number;
  canView: boolean;
  canCreate: boolean;
  canEdit: boolean;
  canDelete: boolean;
}

@Component({
  selector: 'app-surt03',
  standalone: true,
  imports: [
    CommonModule, 
    RouterModule, 
    FormsModule,
    MatTableModule, 
    MatButtonModule, 
    MatCheckboxModule, 
    MatFormFieldModule,
    MatIconModule,
    MatSnackBarModule,
    MatProgressSpinnerModule,
    AppCardComponent,
    ActionBarComponent,
    SelectInputComponent
  ],
  templateUrl: './surt03.component.html',
  styleUrl: './surt03.component.scss'
})
export class Surt03Component implements OnInit {
  private service = inject(Surt03Service);
  private snackBar = inject(MatSnackBar);

  profiles: Profile[] = [];
  selectedProfileId: string | null = null;
  
  displayedColumns: string[] = ['menuName', 'canView', 'canCreate', 'canEdit', 'canDelete', 'actions'];
  dataSource = new MatTableDataSource<MenuPermission>([]);

  isSaving = signal(false);

  ngOnInit() {
    this.loadProfiles();
  }

  loadProfiles() {
    this.service.getProfiles().subscribe(data => {
      this.profiles = data.items;
    });
  }

  loadPermissions() {
    if (!this.selectedProfileId) return;

    this.service.getPermissions(this.selectedProfileId).subscribe(data => {
      this.dataSource.data = data;
    });
  }

  savePermissions() {
    if (!this.selectedProfileId) return;

    this.isSaving.set(true);
    
    // Filter only permissions that have at least one true value (optimization)
    // OR send all. Let's send only relevant ones.
    const permissions = this.dataSource.data.map((p: any) => ({
        menuId: p.menuId,
        canView: p.canView,
        canCreate: p.canCreate,
        canEdit: p.canEdit,
        canDelete: p.canDelete
    }));

    this.service.updatePermissions(this.selectedProfileId, permissions).subscribe({
        next: () => {
            this.snackBar.open('Permissions saved successfully', 'Close', { duration: 3000 });
            this.isSaving.set(false);
        },
        error: () => this.isSaving.set(false),
        complete: () => this.isSaving.set(false)
    });
  }

  toggleRow(element: MenuPermission) {
      const newState = !element.canView;
      element.canView = newState;
      element.canCreate = newState;
      element.canEdit = newState;
      element.canDelete = newState;
  }

  exportData() {
      if (!this.selectedProfileId) {
          this.snackBar.open('Please select a profile to export.', 'Close', { duration: 3000 });
          return;
      }
      this.service.exportPermissions(this.selectedProfileId).subscribe((blob: Blob) => {
          const url = window.URL.createObjectURL(blob);
          const a = document.createElement('a');
          a.href = url;
          a.download = `Permissions_${new Date().getTime()}.xlsx`;
          document.body.appendChild(a);
          a.click();
          window.URL.revokeObjectURL(url);
          a.remove();
      });
  }
}

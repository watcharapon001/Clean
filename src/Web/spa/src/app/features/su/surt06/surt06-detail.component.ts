import { Component, Inject, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Surt06Service, Organize, CreateOrganizeCommand, UpdateOrganizeCommand } from './surt06.service';

@Component({
  selector: 'app-surt06-detail',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule
  ],
  template: `
    <h2 mat-dialog-title>{{ isEditMode ? 'Edit Organization' : 'Create Organization' }}</h2>
    <mat-dialog-content class="py-3">
      <form [formGroup]="orgForm" class="d-flex flex-column gap-3">
        
        <mat-form-field appearance="outline" class="w-100">
          <mat-label>Organization Code</mat-label>
          <input matInput formControlName="orgCode" placeholder="Ex. ORG03" required>
          <mat-error *ngIf="orgForm.get('orgCode')?.hasError('required')">Code is required</mat-error>
        </mat-form-field>

        <mat-form-field appearance="outline" class="w-100">
          <mat-label>Organization Name</mat-label>
          <input matInput formControlName="orgName" placeholder="Ex. IT Department" required>
          <mat-error *ngIf="orgForm.get('orgName')?.hasError('required')">Name is required</mat-error>
        </mat-form-field>

        <mat-form-field appearance="outline" class="w-100">
          <mat-label>Status</mat-label>
          <mat-select formControlName="isActive">
            <mat-option [value]="true">Active</mat-option>
            <mat-option [value]="false">Inactive</mat-option>
          </mat-select>
        </mat-form-field>

      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end" class="px-4 pb-4">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button color="primary" 
              [disabled]="orgForm.invalid || isLoading" 
              (click)="save()">
        {{ isLoading ? 'Saving...' : 'Save' }}
      </button>
    </mat-dialog-actions>
  `
})
export class Surt06DetailComponent {
  private fb = inject(FormBuilder);
  private surt06Service = inject(Surt06Service);
  private snackBar = inject(MatSnackBar);

  orgForm: FormGroup;
  isEditMode: boolean = false;
  isLoading: boolean = false;

  constructor(
    public dialogRef: MatDialogRef<Surt06DetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Organize | null
  ) {
    this.isEditMode = !!data;
    
    this.orgForm = this.fb.group({
      orgCode: [{ value: data?.orgCode || '', disabled: this.isEditMode }, Validators.required],
      orgName: [data?.orgName || '', Validators.required],
      isActive: [data ? data.isActive : true]
    });
  }

  save() {
    if (this.orgForm.invalid) return;

    this.isLoading = true;
    const formValues = this.orgForm.getRawValue(); // gets disabled fields too

    if (this.isEditMode && this.data) {
      const command: UpdateOrganizeCommand = {
        orgId: this.data.orgId,
        orgName: formValues.orgName,
        isActive: formValues.isActive
      };

      this.surt06Service.updateOrganize(command).subscribe({
        next: () => this.handleSuccess('Organization updated successfully'),
        error: (err) => this.handleError(err)
      });
    } else {
      const command: CreateOrganizeCommand = {
        orgCode: formValues.orgCode,
        orgName: formValues.orgName,
        isActive: formValues.isActive
      };

      this.surt06Service.createOrganize(command).subscribe({
        next: () => this.handleSuccess('Organization created successfully'),
        error: (err) => this.handleError(err)
      });
    }
  }

  private handleSuccess(message: string) {
    this.isLoading = false;
    this.snackBar.open(message, 'Close', { duration: 3000 });
    this.dialogRef.close(true);
  }

  private handleError(err: any) {
    this.isLoading = false;
    let errorMsg = 'An error occurred while saving.';
    if (err.error && typeof err.error === 'string') {
        errorMsg = err.error;
    } else if (err.error?.title) {
        errorMsg = err.error.title;
    } else if (err.error?.detail) {
        errorMsg = err.error.detail;
    }

    this.snackBar.open(errorMsg, 'Close', { duration: 5000, panelClass: ['bg-danger', 'text-white'] });
  }
}

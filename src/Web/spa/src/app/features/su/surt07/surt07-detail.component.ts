import { Component, Inject, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Surt07Service, SuConfig, UpdateConfigCommand } from './surt07.service';

@Component({
  selector: 'app-surt07-detail',
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
    <h2 mat-dialog-title>Edit Configuration</h2>
    <mat-dialog-content class="py-3">
      <form [formGroup]="configForm" class="d-flex flex-column gap-3">
        
        <mat-form-field appearance="outline" class="w-100">
          <mat-label>Configuration Key</mat-label>
          <input matInput formControlName="configKey" readonly>
        </mat-form-field>

        <p class="text-muted text-sm my-0">{{ data.description }}</p>

        <!-- Dynamic Input based on DataType -->
        <ng-container [ngSwitch]="data.dataType">
          
          <!-- Boolean Switch -->
          <mat-form-field *ngSwitchCase="'boolean'" appearance="outline" class="w-100">
            <mat-label>Value</mat-label>
            <mat-select formControlName="configValue">
              <mat-option value="true">Enabled (True)</mat-option>
              <mat-option value="false">Disabled (False)</mat-option>
            </mat-select>
          </mat-form-field>

          <!-- Number Input -->
          <mat-form-field *ngSwitchCase="'number'" appearance="outline" class="w-100">
            <mat-label>Value</mat-label>
            <input matInput type="number" formControlName="configValue" required>
            <mat-error *ngIf="configForm.get('configValue')?.hasError('required')">Value is required</mat-error>
          </mat-form-field>

          <!-- Default Text Input -->
          <mat-form-field *ngSwitchDefault appearance="outline" class="w-100">
            <mat-label>Value</mat-label>
            <textarea matInput formControlName="configValue" rows="3" required></textarea>
            <mat-error *ngIf="configForm.get('configValue')?.hasError('required')">Value is required</mat-error>
          </mat-form-field>

        </ng-container>
      </form>
    </mat-dialog-content>
    <mat-dialog-actions align="end" class="px-4 pb-4">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button color="primary" 
              [disabled]="configForm.invalid || isLoading" 
              (click)="save()">
        {{ isLoading ? 'Saving...' : 'Save' }}
      </button>
    </mat-dialog-actions>
  `
})
export class Surt07DetailComponent {
  private fb = inject(FormBuilder);
  private surt07Service = inject(Surt07Service);
  private snackBar = inject(MatSnackBar);

  configForm: FormGroup;
  isLoading: boolean = false;

  constructor(
    public dialogRef: MatDialogRef<Surt07DetailComponent>,
    @Inject(MAT_DIALOG_DATA) public data: SuConfig
  ) {
    this.configForm = this.fb.group({
      configKey: [{ value: data.configKey, disabled: true }],
      configValue: [data.configValue, Validators.required]
    });
  }

  save() {
    if (this.configForm.invalid) return;

    this.isLoading = true;
    const formValues = this.configForm.getRawValue(); 

    const command: UpdateConfigCommand = {
      configKey: this.data.configKey,
      configValue: formValues.configValue.toString()
    };

    this.surt07Service.updateConfig(command).subscribe({
      next: () => {
        this.isLoading = false;
        this.snackBar.open('Configuration updated successfully', 'Close', { duration: 3000 });
        this.dialogRef.close(true);
      },
      error: (err) => {
        this.isLoading = false;
        const errorMsg = err.error?.detail || err.error?.title || 'Error updating configuration';
        this.snackBar.open(errorMsg, 'Close', { duration: 5000, panelClass: ['bg-danger', 'text-white'] });
      }
    });
  }
}

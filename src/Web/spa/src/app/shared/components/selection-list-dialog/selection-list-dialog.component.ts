import { Component, Inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule } from '@angular/forms';

export interface SelectionDialogData {
  title: string;
  items: any[];
  displayField: string;
  subDisplayField?: string;
  disabledItemIds: any[];
  idField: string;
}

@Component({
  selector: 'app-selection-list-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatListModule,
    MatIconModule,
    MatCheckboxModule,
    FormsModule
  ],
  template: `
    <h2 mat-dialog-title>{{ data.title }}</h2>
    <mat-dialog-content>
      <mat-selection-list #selectionList>
        <mat-list-option *ngFor="let item of data.items" 
                         [value]="item"
                         [disabled]="isDisabled(item)"
                         [selected]="isDisabled(item)">
          <div mat-line>{{ item[data.displayField] }}</div>
          <div mat-line *ngIf="data.subDisplayField" class="text-muted small">
            {{ item[data.subDisplayField] }}
          </div>
        </mat-list-option>
      </mat-selection-list>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-raised-button color="primary" (click)="onConfirm()">
        Select
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    mat-dialog-content {
      min-width: 300px;
      max-height: 400px;
    }
  `]
})
export class SelectionListDialogComponent {
  @ViewChild('selectionList') selectionList!: any;

  constructor(
    public dialogRef: MatDialogRef<SelectionListDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: SelectionDialogData
  ) {}

  isDisabled(item: any): boolean {
    return this.data.disabledItemIds.includes(item[this.data.idField]);
  }

  onConfirm() {
    const selectedValues = this.selectionList.selectedOptions.selected.map((option: any) => option.value);
    this.dialogRef.close(selectedValues);
  }
}

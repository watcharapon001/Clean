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
  templateUrl: './selection-list-dialog.component.html',
  styleUrls: ['./selection-list-dialog.component.scss']
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

import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { AuditLog } from './surt05.service';

@Component({
  selector: 'app-json-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MatButtonModule],
  template: `
    <h2 mat-dialog-title>Audit Log Details</h2>
    <mat-dialog-content class="mat-typography">
      <div class="mb-4">
        <strong>Action:</strong> {{data.action}} on <strong>{{data.tableName}}</strong><br>
        <strong>Timestamp:</strong> {{data.timestamp | date:'medium'}}<br>
        <strong>User:</strong> {{data.userId}}
      </div>
      
      <div *ngIf="data.keyValues" class="mb-4">
        <h3 class="text-sm font-semibold text-gray-600 mb-1">Key Values</h3>
        <pre class="bg-gray-100 p-2 rounded text-xs overflow-x-auto">{{formatJson(data.keyValues)}}</pre>
      </div>

      <div *ngIf="data.oldValues" class="mb-4">
        <h3 class="text-sm font-semibold text-gray-600 mb-1">Old Values</h3>
        <pre class="bg-red-50 p-2 rounded text-xs overflow-x-auto border border-red-100">{{formatJson(data.oldValues)}}</pre>
      </div>

      <div *ngIf="data.newValues" class="mb-4">
        <h3 class="text-sm font-semibold text-gray-600 mb-1">New Values</h3>
        <pre class="bg-green-50 p-2 rounded text-xs overflow-x-auto border border-green-100">{{formatJson(data.newValues)}}</pre>
      </div>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close cdkFocusInitial>Close</button>
    </mat-dialog-actions>
  `,
  styles: [`
    pre { margin: 0; white-space: pre-wrap; word-wrap: break-word; }
  `]
})
export class JsonDialogComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: AuditLog) {}

  formatJson(jsonStr: string): string {
    try {
      return JSON.stringify(JSON.parse(jsonStr), null, 2);
    } catch {
      return jsonStr;
    }
  }
}

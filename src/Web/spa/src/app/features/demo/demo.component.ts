import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { TextboxComponent, NumberComponent, SelectInputComponent } from '../../shared/components/inputs';

@Component({
  selector: 'app-demo',
  standalone: true,
  imports: [
    CommonModule, 
    ReactiveFormsModule, 
    MatCardModule, 
    MatButtonModule,
    TextboxComponent,
    NumberComponent,
    SelectInputComponent
  ],
  templateUrl: './demo.component.html',
  styleUrls: ['./demo.component.scss']
})
export class DemoComponent {
  demoForm: FormGroup;
  submittedData: any = null;

  departmentOptions = [
    { id: 'IT', name: 'Information Technology' },
    { id: 'HR', name: 'Human Resources' },
    { id: 'FIN', name: 'Finance' },
    { id: 'MKT', name: 'Marketing' },
    { id: 'SALES', name: 'Sales' },
    { id: 'ENG', name: 'Engineering' }
  ];

  private fb = inject(FormBuilder);

  constructor() {
    this.demoForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],
      age: [null, [Validators.required, Validators.min(1), Validators.max(120)]],
      salary: [null, [Validators.required, Validators.min(100)]],
      department: [null, Validators.required]
    });
  }

  onSubmit() {
    if (this.demoForm.valid) {
      this.submittedData = this.demoForm.getRawValue();
    } else {
      this.demoForm.markAllAsTouched();
    }
  }

  onReset() {
    this.demoForm.reset();
    this.submittedData = null;
  }
}

import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { EmployeeService } from '../employee.service';

@Component({
  selector: 'app-employee-form',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './employee-form.component.html',
  styles: []
})
export class EmployeeFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private employeeService = inject(EmployeeService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  form: FormGroup;
  isEditMode = signal(false);
  employeeId: string | null = null;

  constructor() {
    this.form = this.fb.group({
      employeeCode: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: [''],
      displayName: [''],
      email: ['', [Validators.email]],
      phone: [''],
      isActive: [true]
    });
  }

  ngOnInit() {
    this.employeeId = this.route.snapshot.paramMap.get('id');
    if (this.employeeId && this.employeeId !== 'new') {
      this.isEditMode.set(true);
      this.loadEmployee(this.employeeId);
    }
  }

  loadEmployee(id: string) {
    this.employeeService.getEmployee(id).subscribe(emp => {
      this.form.patchValue(emp);
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    const empData = this.form.value;

    if (this.isEditMode() && this.employeeId) {
      this.employeeService.updateEmployee(this.employeeId, empData).subscribe(() => {
        this.router.navigate(['/db/employees']);
      });
    } else {
      this.employeeService.createEmployee(empData).subscribe(() => {
        this.router.navigate(['/db/employees']);
      });
    }
  }
}

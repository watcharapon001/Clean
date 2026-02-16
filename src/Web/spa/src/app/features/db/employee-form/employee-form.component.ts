import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { EmployeeService } from '../employee.service';
import { LoadingService } from '../../../core/services/loading.service';

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
  private loadingService = inject(LoadingService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  form: FormGroup;
  isEditMode = signal(false);
  isLoading = this.loadingService.isLoading;
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

    this.loadingService.show();
    const empData = this.form.value;

    const action$: Observable<unknown> = (this.isEditMode() && this.employeeId)
      ? this.employeeService.updateEmployee(this.employeeId, { ...empData, employeeId: this.employeeId })
      : this.employeeService.createEmployee(empData);

    action$.pipe(
      finalize(() => this.loadingService.hide())
    ).subscribe({
      next: () => {
        this.router.navigate(['/db/employees']);
      },
      error: (err: any) => {
        console.error('Error saving employee', err);
        // Optionally handle error (e.g. show toast)
      }
    });
  }
}

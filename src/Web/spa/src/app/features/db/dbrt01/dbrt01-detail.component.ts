import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { EmployeeService } from './employee.service';
import { LoadingService } from '../../../core/services/loading.service';
import { AppCardComponent } from '../../../shared/components/card/card.component';

@Component({
  selector: 'app-dbrt01-detail',
  standalone: true,
  imports: [
    CommonModule, 
    RouterModule, 
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatCheckboxModule,
    MatCardModule,
    MatIconModule,
    MatProgressSpinnerModule,
    AppCardComponent
  ],
  templateUrl: './dbrt01-detail.component.html',
  styleUrls: ['./dbrt01-detail.component.scss']
})
export class Dbrt01DetailComponent implements OnInit {
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
        this.router.navigate(['/db/dbrt01']);
      },
      error: (err: any) => {
        console.error('Error saving employee', err);
        // Optionally handle error (e.g. show toast)
      }
    });
  }
}

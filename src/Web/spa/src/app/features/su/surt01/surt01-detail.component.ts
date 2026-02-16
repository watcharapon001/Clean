import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { AppCardComponent } from '../../../shared/components/card/card.component';
import { Profile } from './surt01.component';
import { Surt01Service } from './surt01.service';

@Component({
  selector: 'app-surt01-detail',
  standalone: true,
  imports: [
    CommonModule, 
    RouterModule, 
    ReactiveFormsModule,
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatCardModule,
    MatIconModule,
    AppCardComponent
  ],
  templateUrl: './surt01-detail.component.html',
  styleUrl: './surt01-detail.component.scss'
})
export class Surt01DetailComponent implements OnInit {
  private fb = inject(FormBuilder);
  private service = inject(Surt01Service);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  form: FormGroup;
  isEditMode = signal(false);
  isSaving = signal(false);
  profileId: string | null = null;

  constructor() {
    this.form = this.fb.group({
      profileCode: ['', Validators.required],
      profileName: ['', Validators.required],
      description: [''],
      isActive: [true]
    });
  }

  ngOnInit() {
    this.profileId = this.route.snapshot.paramMap.get('id');
    if (this.profileId && this.profileId !== 'new') {
      this.isEditMode.set(true);
      this.loadProfile(this.profileId);
    }
  }

  loadProfile(id: string) {
    this.service.getProfile(id).subscribe(data => {
      this.form.patchValue(data);
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.isSaving.set(true);
    const formData = this.form.value;

    const request$: Observable<any> = (this.isEditMode() && this.profileId)
      ? this.service.updateProfile(this.profileId, { ...formData, profileId: this.profileId })
      : this.service.createProfile(formData);

    request$.subscribe({
      next: () => {
        this.router.navigate(['/su/surt01']);
      },
      error: () => this.isSaving.set(false),
      complete: () => this.isSaving.set(false)
    });
  }
}

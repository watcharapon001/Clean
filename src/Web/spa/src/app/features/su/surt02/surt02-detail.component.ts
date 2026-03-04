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
import { ActionBarComponent } from '../../../shared/components/action-bar/action-bar.component';
import { TextboxComponent, NumberComponent, SelectInputComponent } from '../../../shared/components/inputs';
import { Menu } from './surt02.component';
import { Surt02Service } from './surt02.service';

@Component({
  selector: 'app-surt02-detail',
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
    AppCardComponent,
    ActionBarComponent,
    TextboxComponent,
    NumberComponent,
    SelectInputComponent
  ],
  templateUrl: './surt02-detail.component.html',
  styleUrl: './surt02-detail.component.scss'
})
export class Surt02DetailComponent implements OnInit {
  private fb = inject(FormBuilder);
  private service = inject(Surt02Service);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  form: FormGroup;
  isEditMode = signal(false);
  isSaving = signal(false);
  menuId: string | null = null;
  parentMenus: Menu[] = [];

  constructor() {
    this.form = this.fb.group({
      menuCode: ['', Validators.required],
      menuName: ['', Validators.required],
      parentMenuId: [null],
      route: [''],
      icon: [''],
      sequence: [1],
      isActive: [true]
    });
  }

  ngOnInit() {
    this.menuId = this.route.snapshot.paramMap.get('id');
    
    this.route.data.subscribe(({ detailData }) => {
      if (detailData) {
        // Filter out self to avoid circular dependency
        this.parentMenus = detailData.parentMenus.filter((m: Menu) => m.menuId !== this.menuId);
        
        if (detailData.menu) {
          this.isEditMode.set(true);
          this.form.patchValue(detailData.menu);
        }
      }
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.isSaving.set(true);
    const formData = this.form.value;

    const request$: Observable<unknown> = (this.isEditMode() && this.menuId)
      ? this.service.updateMenu(this.menuId, { ...formData, menuId: this.menuId })
      : this.service.createMenu(formData);

    request$.subscribe({
      next: () => {
        this.router.navigate(['/su/surt02']);
      },
      error: () => this.isSaving.set(false),
      complete: () => this.isSaving.set(false)
    });
  }
}

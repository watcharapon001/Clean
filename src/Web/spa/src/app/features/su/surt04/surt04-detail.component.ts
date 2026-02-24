import { Component, inject, signal, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Observable, forkJoin } from 'rxjs';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTabsModule } from '@angular/material/tabs';
import { MatTableModule } from '@angular/material/table';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AppCardComponent } from '../../../shared/components/card/card.component';
import { SelectionListDialogComponent } from '../../../shared/components/selection-list-dialog/selection-list-dialog.component';
import { ActionBarComponent } from '../../../shared/components/action-bar/action-bar.component';
import { TextboxComponent, SelectInputComponent } from '../../../shared/components/inputs';
import { Surt04Service, Employee, Organization, UserOrg } from './surt04.service';
import { Surt01Service } from '../surt01/surt01.service';
import { Profile } from '../surt01/surt01.component';

@Component({
  selector: 'app-surt04-detail',
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
    MatProgressSpinnerModule,
    MatTabsModule,
    MatTableModule,
    MatDialogModule,
    AppCardComponent,
    ActionBarComponent,
    TextboxComponent,
    SelectInputComponent
  ],
  templateUrl: './surt04-detail.component.html',
  styleUrl: './surt04-detail.component.scss'
})
export class Surt04DetailComponent implements OnInit {
  private fb = inject(FormBuilder);
  private service = inject(Surt04Service);
  private profileService = inject(Surt01Service);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private dialog = inject(MatDialog);
  private cd = inject(ChangeDetectorRef);

  form: FormGroup;
  isEditMode = signal(false);
  isSaving = signal(false);
  isLoading = signal(false);
  userId: string | null = null;

  employees: Employee[] = [];
  profiles: Profile[] = [];
  organizations: Organization[] = [];

  // Selected items for display
  selectedProfiles: Profile[] = [];
  selectedOrgs: Organization[] = [];
  displayedProfileColumns: string[] = ['code', 'name', 'actions'];
  displayedOrgColumns: string[] = ['code', 'name', 'actions'];

  constructor() {
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: [''], // Required for new, optional for edit (handled in validation logic if needed)
      email: ['', [Validators.required, Validators.email]],
      employeeId: [null], // Optional
      profileIds: [[]],
      userOrgs: [[]],
      isActive: [true]
    });
  }

  ngOnInit() {
    this.isLoading.set(true);
    
    // Load dependencies first
    forkJoin({
        employees: this.service.getEmployees(),
        profiles: this.profileService.getProfiles(),
        orgs: this.service.getOrganizes()
    }).subscribe({
        next: (results) => {
            this.employees = results.employees;
            this.profiles = results.profiles;
            this.organizations = results.orgs;
            
            this.checkAndLoadUser();
        },
        error: () => this.isLoading.set(false)
    });
  }

  checkAndLoadUser() {
    this.userId = this.route.snapshot.paramMap.get('id');
    if (this.userId && this.userId !== 'new') {
      this.isEditMode.set(true);
      this.loadUser(this.userId);
    } else {
        // New user: Password is required
        this.form.get('password')?.setValidators([Validators.required]);
        this.form.get('password')?.updateValueAndValidity();
        this.isLoading.set(false);
    }
  }

  // Removed loadDependencies method as it's handled in ngOnInit
  // loadDependencies() { ... }

  loadUser(id: string) {
    this.isLoading.set(true);
    this.service.getUser(id).subscribe({
      next: (data) => {
        this.form.patchValue({
            username: data.username,
            email: data.email,
            employeeId: data.employeeId,
            profileIds: data.profileIds,
            userOrgs: data.userOrgs ? data.userOrgs.map(uo => uo.orgId) : [],
            isActive: data.isActive,
            password: '' // Don't show password
        });

        // Populate displayed lists
        if (data.profileIds) {
            this.selectedProfiles = this.profiles.filter(p => data.profileIds.includes(p.profileId));
        }
        if (data.userOrgs) {
            const orgIds = data.userOrgs.map(uo => uo.orgId);
            this.selectedOrgs = this.organizations.filter(o => orgIds.includes(o.orgId));
        }

        this.isLoading.set(false);
      },
      error: () => this.isLoading.set(false)
    });
  }

  openProfileDialog() {
    const dialogRef = this.dialog.open(SelectionListDialogComponent, {
      width: '400px',
      data: {
        title: 'Select Profiles',
        items: this.profiles,
        displayField: 'profileName',
        subDisplayField: 'profileCode',
        disabledItemIds: this.selectedProfiles.map(p => p.profileId),
        idField: 'profileId'
      }
    });

    dialogRef.afterClosed().subscribe((result: any) => {
        if (result && result.length > 0) {
            // Robust deduplication using Map
            const merged = [...this.selectedProfiles, ...result];
            const uniqueMap = new Map();
            merged.forEach(item => uniqueMap.set(item.profileId, item));
            
            this.selectedProfiles = Array.from(uniqueMap.values());
            this.updateProfileIds();
            this.cd.detectChanges();
        }
    });
  }

  removeProfile(profile: Profile) {
    this.selectedProfiles = this.selectedProfiles.filter(p => p.profileId !== profile.profileId);
    this.updateProfileIds();
    this.cd.detectChanges();
  }

  updateProfileIds() {
    this.form.patchValue({
        profileIds: this.selectedProfiles.map(p => p.profileId)
    });
  }

  openOrgDialog() {
    const dialogRef = this.dialog.open(SelectionListDialogComponent, {
      width: '400px',
      data: {
        title: 'Select Organizations',
        items: this.organizations,
        displayField: 'orgName',
        subDisplayField: 'orgCode',
        disabledItemIds: this.selectedOrgs.map(o => o.orgId),
        idField: 'orgId'
      }
    });

    dialogRef.afterClosed().subscribe((result: any) => {
        if (result && result.length > 0) {
            // Robust deduplication using Map
            const merged = [...this.selectedOrgs, ...result];
            const uniqueMap = new Map();
            merged.forEach(item => uniqueMap.set(item.orgId, item));
            
            this.selectedOrgs = Array.from(uniqueMap.values());
            this.updateOrgIds();
            this.cd.detectChanges();
        }
    });
  }

  removeOrg(org: Organization) {
    this.selectedOrgs = this.selectedOrgs.filter(o => o.orgId !== org.orgId);
    this.updateOrgIds();
    this.cd.detectChanges();
  }

  updateOrgIds() {
    this.form.patchValue({
        userOrgs: this.selectedOrgs.map(o => o.orgId)
    });
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.isSaving.set(true);
    const formData = this.form.value;
    
    // If edit mode and password is empty, remove it from payload so it's not updated
    if (this.isEditMode() && !formData.password) {
        delete formData.password;
    }

    // Map selected org IDs to UserOrg objects
    const selectedOrgIds: string[] = formData.userOrgs || [];
    const userOrgs: UserOrg[] = selectedOrgIds.map(orgId => {
        const org = this.organizations.find(o => o.orgId === orgId);
        return {
            orgId: orgId,
            orgCode: org?.orgCode || '',
            orgName: org?.orgName || '',
            isDefault: false // Default to false for now, UI can be enhanced later to select default
        };
    });

    const payload = { ...formData, userOrgs };

    const request$: Observable<any> = (this.isEditMode() && this.userId)
      ? this.service.updateUser(this.userId, { ...payload, userId: this.userId })
      : this.service.createUser(payload);

    request$.subscribe({
      next: () => {
        this.router.navigate(['/su/surt04']);
      },
      error: () => this.isSaving.set(false),
      complete: () => this.isSaving.set(false)
    });
  }
}

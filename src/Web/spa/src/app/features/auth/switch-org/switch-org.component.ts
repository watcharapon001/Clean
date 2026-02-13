import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Organization } from '../../../core/auth/auth.service';
import { Router } from '@angular/router';
import { SwitchOrgService } from './switch-org.service';

@Component({
  selector: 'app-switch-org',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './switch-org.component.html',
  styleUrls: ['./switch-org.component.scss']
})
export class SwitchOrgComponent {
  private switchOrgService = inject(SwitchOrgService);
  private router = inject(Router);

  organizations = signal<Organization[]>([]);
  isLoading = signal(true);
  errorMessage = signal<string | null>(null);

  constructor() {
    this.switchOrgService.getOrgs().subscribe({
      next: (orgs) => {
        this.organizations.set(orgs);
        this.isLoading.set(false);
      },
      error: (err) => {
        this.errorMessage.set('Failed to load organizations.');
        this.isLoading.set(false);
        console.error(err);
      }
    });
  }

  isCurrentOrg(orgId: string): boolean {
    return this.switchOrgService.currentOrg()?.orgId === orgId;
  }

  switchOrg(orgId: string) {
    if (this.isCurrentOrg(orgId)) return;

    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.switchOrgService.switchOrg(orgId).subscribe({
      next: () => {
        this.isLoading.set(false);
        this.router.navigate(['/']); // Redirect to home/dashboard
      },
      error: (err) => {
        this.isLoading.set(false);
        this.errorMessage.set('Failed to switch organization.');
        console.error(err);
      }
    });
  }

  goBack() {
    this.router.navigate(['/']);
  }
}

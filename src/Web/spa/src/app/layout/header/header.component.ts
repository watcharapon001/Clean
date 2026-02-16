import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { Organization } from '../../core/auth/auth.service';
import { HeaderService } from './header.service';
import { MainLayoutService } from '../main-layout/main-layout.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  private headerService = inject(HeaderService);
  public mainLayoutService = inject(MainLayoutService);
  private router = inject(Router);

  organizations = signal<Organization[]>([]);
  currentOrg = this.headerService.currentOrg;
  showOrgMenu = signal(false);
  currentUser = this.headerService.currentUser;

  ngOnInit() {
    this.loadOrgs();
  }

  loadOrgs() {
    this.headerService.getOrgs().subscribe(orgs => {
      this.organizations.set(orgs);
    });
  }

  toggleOrgMenu() {
    this.showOrgMenu.update(v => !v);
  }

  switchOrg(orgId: string) {
    this.headerService.switchOrg(orgId).subscribe(() => {
      this.showOrgMenu.set(false);
      this.router.navigate(['/']); 
    });
  }

  logout() {
    this.headerService.logout();
  }
}

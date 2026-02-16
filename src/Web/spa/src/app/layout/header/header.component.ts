import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule, NavigationEnd, ActivatedRoute } from '@angular/router';
import { Organization } from '../../core/auth/auth.service';
import { HeaderService } from './header.service';
import { MainLayoutService } from '../main-layout/main-layout.service';
import { filter, map } from 'rxjs/operators';

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
  private activatedRoute = inject(ActivatedRoute);

  organizations = signal<Organization[]>([]);
  currentOrg = this.headerService.currentOrg;
  showOrgMenu = signal(false);
  currentUser = this.headerService.currentUser;
  currentProgramCode = signal<string>('');

  ngOnInit() {
    this.loadOrgs();
    this.subscribeToRouterEvents();
  }

  private subscribeToRouterEvents() {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(() => {
        let route = this.activatedRoute;
        while (route.firstChild) {
          route = route.firstChild;
        }
        return route;
      })
    ).subscribe(route => {
      const programCode = route.snapshot.data['programCode'] || '';
      this.currentProgramCode.set(programCode);
    });
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

import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, Router, Event, NavigationStart, NavigationEnd, NavigationCancel, NavigationError } from '@angular/router';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { HeaderComponent } from '../header/header.component';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { MainLayoutService } from './main-layout.service';

@Component({
  selector: 'app-main-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, HeaderComponent, SidebarComponent, MatProgressBarModule],
  templateUrl: './main-layout.component.html',
  styleUrls: ['./main-layout.component.scss']
})
export class MainLayoutComponent {
  mainLayoutService = inject(MainLayoutService);
  private router = inject(Router);
  isLoadingRoute = signal(false);

  constructor() {
    this.router.events.subscribe((event: Event) => {
      if (event instanceof NavigationStart) {
        this.isLoadingRoute.set(true);
      } else if (
        event instanceof NavigationEnd ||
        event instanceof NavigationCancel ||
        event instanceof NavigationError
      ) {
        this.isLoadingRoute.set(false);
      }
    });
  }
}

import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MainLayoutService {
  readonly isSidebarOpen = signal(true);

  toggleSidebar() {
    this.isSidebarOpen.update(v => !v);
  }
}

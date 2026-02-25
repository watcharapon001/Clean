import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SidebarService } from './sidebar.service';
import { Menu } from './menu.model';
import { HeaderService } from '../header/header.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss'],
})
export class SidebarComponent implements OnInit {
  private sidebarService = inject(SidebarService);
  private headerService = inject(HeaderService);

  menus = signal<Menu[]>([]);
  currentUser = this.headerService.currentUser;
  expandedMenus = signal<Set<string>>(new Set());

  ngOnInit() {
    this.sidebarService.getMenus().subscribe((data) => {
      this.menus.set(data);
    });
  }

  toggleMenu(menuId: string): void {
    this.expandedMenus.update(set => {
      const newSet = new Set(set);
      if (newSet.has(menuId)) {
        newSet.delete(menuId);
      } else {
        newSet.add(menuId);
      }
      return newSet;
    });
  }

  isExpanded(menuId: string): boolean {
    return this.expandedMenus().has(menuId);
  }
}

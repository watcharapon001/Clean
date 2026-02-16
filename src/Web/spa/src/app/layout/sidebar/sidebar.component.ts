import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SidebarService } from './sidebar.service';
import { Menu } from './menu.model';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {
  private sidebarService = inject(SidebarService);
  
  menus = signal<Menu[]>([]);

  ngOnInit() {
    this.sidebarService.getMenus().subscribe(data => {
      this.menus.set(data);
    });
  }
}

import { Component } from '@angular/core';
import { SidebarService } from '../sidebar.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  standalone: true
})
export class HeaderComponent {
  isSidebarCollapsed = false;

  constructor(private sidebarService: SidebarService) {
    this.sidebarService.sidebarCollapsed$.subscribe(
      (collapsed) => this.isSidebarCollapsed = collapsed
    );
  }

  toggleSidebar() {
    this.sidebarService.toggleSidebar();
  }
}

import { Component } from '@angular/core';
import { SidebarService } from '../sidebar.service';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css'],
  standalone: true,
  imports : [RouterModule,CommonModule]
})
export class SidebarComponent {
  isSidebarCollapsed = false;
  isSubMenuOpen = false;

  constructor(private sidebarService: SidebarService) {
    this.sidebarService.sidebarCollapsed$.subscribe(
      (collapsed) => this.isSidebarCollapsed = collapsed
    );
  }

  toggleSubMenu() {
    this.isSubMenuOpen = !this.isSubMenuOpen;
  }
}

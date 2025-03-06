import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../auth/auth.service';
import { NgIconComponent } from '@ng-icons/core';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, NgIconComponent],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent {
  userName: string | null = null;
  isSidebarOpen = false;

  constructor(
    public authService: AuthService,
    public router: Router
  ) {
    this.userName = this.authService.getUserName();
  }

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  navigateTo(path: string) {
    this.router.navigate([`/admin-dashboard/${path}`]);
    this.isSidebarOpen = false;
  }
}
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../auth/auth.service';
import { NgIconComponent } from '@ng-icons/core';

@Component({
  selector: 'app-operator-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, NgIconComponent],
  templateUrl: './operator-dashboard.component.html',
  styleUrls: ['./operator-dashboard.component.css']
})
export class OperatorDashboardComponent {
  userName: string | null = null;
  isSidebarOpen = false;

  constructor(
    private authService: AuthService,
    public router: Router
  ) {
    this.userName = this.authService.getUserName();
  }

  toggleSidebar() {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  navigateTo(path: string) {
    this.router.navigate([`/operator-dashboard/${path}`]);
    this.isSidebarOpen = false;
  }
}
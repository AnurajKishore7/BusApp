import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgIconComponent } from '@ng-icons/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../auth/auth.service';
import { Router } from '@angular/router';
import { Login } from '../core/models/login.dto';
import { PopupService } from '../auth/popup.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, NgIconComponent, FormsModule, RouterModule],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  showPopup = false;
  isLoggedIn = false;
  loginData: Login = { email: '', password: '' };
  errorMessage: string | null = null;
  showDropdown = false;
  userName: string | null = null;

  constructor(
    private authService: AuthService,
    private router: Router,
    private popupService: PopupService
  ) {
    this.authService.isLoggedIn().subscribe(loggedIn => {
      this.isLoggedIn = loggedIn;
      if (loggedIn) {
        this.userName = this.authService.getUserName(); 
      }
    });
    this.popupService.openLoginPopup$.subscribe(() => {
      this.showPopup = true;
      this.loginData = { email: '', password: '' };
      this.errorMessage = null;
    });
  }

  togglePopup() {
    this.showPopup = !this.showPopup;
    if (this.showPopup) {
      this.loginData = { email: '', password: '' };
    }
    this.errorMessage = null;
    this.showDropdown = false;
  }

  onLogin() {
    this.authService.login(this.loginData).subscribe({
      next: (response) => {
        localStorage.setItem('token', response.token || '');
        this.isLoggedIn = true;
        this.showPopup = false;
        this.loginData = { email: '', password: '' };
        this.errorMessage = null;
  
        const role = response.role;
        if (role === 'TransportOperator') {
          this.router.navigate(['/operator-dashboard']);
        } else if (role === 'Admin') {
          this.router.navigate(['/admin-dashboard']);
        } else if (role === 'Client') {
          this.router.navigate(['/trip-search']);
        } else {
          this.router.navigate(['/']); 
        }
      },
      error: (err) => {
        this.errorMessage = err.error?.message || 'Login failed';
      }
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/']);
    this.showDropdown = false;
  }

  navigateTo(path: string) {
    this.showPopup = false;
    this.showDropdown = false;
    this.router.navigate([path]);
  }

  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
    this.showPopup = false;
  }
}
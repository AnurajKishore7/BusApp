import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean {
    const token = this.authService.getToken();
    if (token) {
      return true; // Logged in
    }
    this.router.navigate(['/']); // Redirect to landing if not logged in
    return false;
  }
}
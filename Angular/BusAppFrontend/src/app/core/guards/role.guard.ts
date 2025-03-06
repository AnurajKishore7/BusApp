import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthService } from '../../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
    const expectedRole = route.data['role'];
    return this.authService.getRole().pipe(
      map(userRole => {
        if (userRole && expectedRole === userRole) {
          return true;
        }
        this.router.navigate(['/']);
        return false;
      })
    );
  }
}
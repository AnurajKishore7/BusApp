// payment.guard.ts
import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../../auth/auth.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PaymentGuard implements CanActivate {
  constructor(
    private http: HttpClient,
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
    const bookingId = route.queryParams['bookingId'];
    const token = this.authService.getToken();

    return this.http
      .get<any>(`http://localhost:5205/api/booking/${bookingId}`, {
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        }
      })
      .pipe(
        map(response => {
          if (response.paymentStatus === 'Completed') {
            alert('Payment already completed for this booking.');
            this.router.navigate(['/']);
            return false;
          }
          return true;
        })
      );
  }
}
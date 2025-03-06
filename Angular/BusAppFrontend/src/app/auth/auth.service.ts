import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { environment } from '../../environments/environment.development';
import { Login } from '../core/models/login.dto';
import { ClientRegister } from '../core/models/client-register.dto';
import { TransportRegister } from '../core/models/transport-register.dto';
import { LoginResponse } from '../core/models/login-response.dto';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiUrl;
  private tokenKey = 'auth_token';
  private loggedInSubject = new BehaviorSubject<boolean>(false);
  private roleSubject = new BehaviorSubject<string | null>(null);

  constructor(private http: HttpClient) {
    const token = localStorage.getItem(this.tokenKey);
    if (token) {
      this.loggedInSubject.next(true);
      const role = this.getRoleFromToken(token);
      this.roleSubject.next(role);
    }
  }

  login(credentials: Login): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/auth/login`, credentials).pipe(
      tap(response => {
        if (response.token) {
          this.setToken(response.token);
          localStorage.setItem('userName', response.name);
          this.loggedInSubject.next(true);
          const token = this.getToken();
          this.roleSubject.next(token ? this.getRoleFromToken(token) : null); // Null check
        }
      })
    );
  }

  registerClient(client: ClientRegister): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/auth/register-client`, client).pipe(
      tap(response => {
        if (response.token) {
          this.setToken(response.token);
          localStorage.setItem('userName', response.name);
          this.loggedInSubject.next(true);
          const token = this.getToken();
          this.roleSubject.next(token ? this.getRoleFromToken(token) : null);
        }
      })
    );
  }

  registerOperator(operator: TransportRegister): Observable<string> {
    return this.http.post<string>(`${this.apiUrl}/auth/register-transport-operator`, operator, { responseType: 'text' as 'json' });
  }

  private setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  private getRoleFromToken(token: string): string | null {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
    } catch (e) {
      return null;
    }
  }

  isLoggedIn(): Observable<boolean> {
    return this.loggedInSubject.asObservable();
  }

  getRole(): Observable<string | null> {
    return this.roleSubject.asObservable();
  }

  getUserName(): string | null {
    return localStorage.getItem('userName');
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem('userName');
    this.loggedInSubject.next(false);
    this.roleSubject.next(null);
  }
}
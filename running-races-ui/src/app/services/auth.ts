import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);

  private apiUrl = 'https://localhost:7156/api/auth';
  private tokenKey = 'jwt_token';

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor() {
    // Initialize auth state from localStorage
    const hasToken = this.hasToken();
    this.isAuthenticatedSubject.next(hasToken);
  }

  login(email: string, password: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, { email, password })
      .pipe(
        tap((response: LoginResponse) => {
          localStorage.setItem(this.tokenKey, response.token);
          const savedToken = localStorage.getItem(this.tokenKey);
          this.isAuthenticatedSubject.next(true);
        })
      );
  }

  logout(): void {
    const token = this.getToken();

    if (token) {
      this.http.post(`${this.apiUrl}/logout`, {}).subscribe({
        next: () => {
          localStorage.removeItem(this.tokenKey);
          this.isAuthenticatedSubject.next(false);
          this.router.navigate(['/races']);
        },
        error: () => {
          // Token invalid/expired, remove anyway
          localStorage.removeItem(this.tokenKey);
          this.isAuthenticatedSubject.next(false);
          this.router.navigate(['/races']);
        }
      });
    } else {
      this.router.navigate(['/races']);
    }
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    return this.hasToken();
  }

  private hasToken(): boolean {
    return !!this.getToken();
  }

  /**
 * Get user role from JWT token
 */
  getUserRole(): string | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token);
      return decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || null;
    } catch {
      return null;
    }
  }

  /**
 * Check if user is admin
 */
  isAdmin(): boolean {
    return this.getUserRole() === 'Admin';
  }
}

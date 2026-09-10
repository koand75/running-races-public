import { Injectable, NgZone, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth';
import { MatSnackBar } from '@angular/material/snack-bar';
import { jwtDecode } from 'jwt-decode';

@Injectable({ providedIn: 'root' })
export class IdleService {
  private router = inject(Router);
  private authService = inject(AuthService);
  private zone = inject(NgZone);
  private snackBar = inject(MatSnackBar);
  private idleTimer: any;
  private warningTimer: any;

  start(): void {
    ['mousemove', 'keydown', 'click', 'scroll'].forEach(event => {
      window.addEventListener(event, () => {
        const token = localStorage.getItem('jwt_token');
        if (!token) return;
        const decoded: any = jwtDecode(token);
        const expiresIn = (decoded.exp * 1000) - Date.now();
        if (expiresIn < 120000) {
          // 5 percen belül lejár → automatikus refresh
          this.authService.refresh().subscribe(res => {
            localStorage.setItem('jwt_token', res.token);
            this.resetTimer();
          });
        }
      });
    });
    this.resetTimer();
  }

  resetTimer(): void {
    clearTimeout(this.idleTimer);
    clearTimeout(this.warningTimer);

    const token = localStorage.getItem('jwt_token');
    if (!token) return;

    const decoded: any = jwtDecode(token);
    console.log('expires in:', (decoded.exp * 1000) - Date.now(), 'ms');
    const expiresIn = (decoded.exp * 1000) - Date.now();
    const warningAt = expiresIn - 60000;

    this.zone.runOutsideAngular(() => {
      this.warningTimer = setTimeout(() => {
        this.zone.run(() => {
          const snackRef = this.snackBar.open('Hamarosan kijelentkeztetünk', 'Maradok', { duration: 55000 });
          snackRef.onAction().subscribe(() => {
            this.authService.refresh().subscribe(res => {
              localStorage.setItem('jwt_token', res.token);
              this.resetTimer();
            });
          });
        });
      }, Math.max(warningAt, 0));

      this.idleTimer = setTimeout(() => {
        this.zone.run(() => {
          this.snackBar.dismiss();
          this.authService.logout();
          this.router.navigate(['/login']);
        });
      }, Math.max(expiresIn, 0));
    });
  }

  stop(): void {
    clearTimeout(this.idleTimer);
    clearTimeout(this.warningTimer);
  }
}
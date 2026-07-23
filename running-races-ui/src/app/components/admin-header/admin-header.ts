import { Component, inject, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth';
import { Subscription } from 'rxjs';
import { MatIconModule } from '@angular/material/icon';
import { Output, EventEmitter } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-admin-header',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule],  // ← async pipe kell
  templateUrl: './admin-header.html',
  styleUrl: './admin-header.css'
})
export class AdminHeaderComponent implements OnInit, OnDestroy {
  @Output() menuToggled = new EventEmitter<void>();

  toggleMenu(): void {
    this.menuToggled.emit();
  }

  public authService = inject(AuthService);
  private router = inject(Router);

  isAuthenticated$ = this.authService.isAuthenticated$;  // ← Observable
  private subscription?: Subscription;

  ngOnInit(): void {
    // Redirect if not authenticated
    this.subscription = this.isAuthenticated$.subscribe(isAuth => {
      if (!isAuth) {
        this.router.navigate(['/login']);
      }
    });
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }

  onLogout(): void {
    this.authService.logout();
  }

  goToAdminHome(): void {
    this.router.navigate(['/admin/races']);
  }
}
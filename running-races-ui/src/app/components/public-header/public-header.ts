import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, NavigationEnd } from '@angular/router';
import { AuthService } from '../../services/auth';
import { MatIconModule } from '@angular/material/icon';
import { filter, map, startWith } from 'rxjs/operators';
import { Output, EventEmitter } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-public-header',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatButtonModule],
  templateUrl: './public-header.html',
  styleUrl: './public-header.css'
})

export class PublicHeaderComponent {
  @Output() menuToggled = new EventEmitter<void>();

  toggleMenu(): void {
    this.menuToggled.emit();
  }

  public authService = inject(AuthService);
  private router = inject(Router);

  isAuthenticated$ = this.authService.isAuthenticated$;
  isLoginPage$ = this.router.events.pipe(
    filter(event => event instanceof NavigationEnd),
    map((event: NavigationEnd) => event.url.includes('login')),
    startWith(this.router.url.includes('login'))
  );

  goToHome(): void {
    this.router.navigate(['/races']);
  }

  onLogout(): void {
    this.authService.logout();
  }

}
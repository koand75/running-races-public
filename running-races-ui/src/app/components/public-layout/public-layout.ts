import { Component, inject } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatListModule } from '@angular/material/list';
import { PublicHeaderComponent } from '../public-header/public-header';
import { AuthService } from '../../services/auth';
import { AsyncPipe } from '@angular/common';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-public-layout',
  standalone: true,
  imports: [RouterOutlet, PublicHeaderComponent, MatSidenavModule,
    MatIconModule, MatButtonModule,
    MatListModule, RouterLink,
    AsyncPipe, MatDividerModule],
  templateUrl: './public-layout.html',
  styleUrl: './public-layout.css'
})
export class PublicLayoutComponent {
  private authService = inject(AuthService);
  isAuthenticated$ = this.authService.isAuthenticated$;
  isAdmin(): boolean { return this.authService.isAdmin(); }

  onLogout(): void {
    this.authService.logout();
  }

  menuOpen = false;

  toggleMenu(): void {
    this.menuOpen = !this.menuOpen;
  }

  closeMenu(): void {
    this.menuOpen = false;
  }
}
import { Component, inject } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDivider, MatListModule } from '@angular/material/list';
import { AdminHeaderComponent } from '../admin-header/admin-header';
import { AuthService } from '../../services/auth';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [RouterOutlet, AdminHeaderComponent,
    MatSidenavModule, MatIconModule,
    MatButtonModule, MatListModule,
    RouterLink, MatDivider, MatDividerModule],
  templateUrl: './admin-layout.html',
  styleUrl: './admin-layout.css'
})
export class AdminLayoutComponent {

  private authService = inject(AuthService);

  logout(): void {
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
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { IdleService } from './services/idle.service.ts';
import { AuthService } from './services/auth';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: '<router-outlet></router-outlet>',
  styleUrl: './app.css'
})
export class AppComponent implements OnInit {
  private idleService = inject(IdleService);
  private authService = inject(AuthService);

  ngOnInit(): void {
     console.log('AppComponent init, authenticated:', this.authService.isAuthenticated());

    if (this.authService.isAuthenticated()) {
      this.idleService.start();
    }
  }
}
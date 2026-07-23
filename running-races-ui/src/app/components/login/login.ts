import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth';
import { HttpErrorResponse } from '@angular/common/http';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, MatIconModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  loginForm: FormGroup;
  error = '';
  loading = false;
  showPassword = false;
  private returnUrl: string;

  constructor() {
    this.loginForm = this.fb.group({
      email: ['', Validators.required],
      password: ['', Validators.required]
    });
    // Query param kiolvasása: /login?returnUrl=/admin/races
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/admin/races';
  }


  onSubmit(): void {
    if (this.loginForm.invalid) {
      return;
    }

    this.loading = true;
    this.error = '';

    const { email, password } = this.loginForm.value;

    this.authService.login(email, password).subscribe({
      next: () => {
        // ✅ Sikeres login → Redirect a returnUrl-re
        this.router.navigate([this.returnUrl]);
      },
      error: (err: HttpErrorResponse) => {
        this.loading = false;

        if (err.status === 401) {
          this.error = 'Hibás email cím vagy jelszó';
        } else if (err.status === 400) {
          this.error = err.error?.message || 'Érvénytelen adatok';
        } else if (err.status === 0) {
          this.error = 'Nem érhető el a szerver. Próbálja később!';
        } else {
          this.error = 'Váratlan hiba történt. Kérjük, értesítse az adminisztrátort!';
        }

        console.error('Login error:', err);
      }
    });
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }
}
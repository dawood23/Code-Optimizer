import { Component, inject, ViewEncapsulation, signal } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink,  } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';
import { AuthRequest } from '../../../models/auth-request';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class LoginComponent {

  router = inject(Router);
  authService = inject(AuthService);

  isSubmitting = signal<boolean>(false);
  errorMessage = signal<string>('');

  loginForm: FormGroup;
  showPassword = false;

  authRequeset: AuthRequest = { username: '', passwordHash: '' };

  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  get username() {
    return this.loginForm.get('username');
  }

  get password() {
    return this.loginForm.get('password');
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {
    if (this.loginForm.valid) {

      this.isSubmitting.set(true);
      this.errorMessage.set('');

      this.authRequeset.username = this.loginForm.value.username;
      this.authRequeset.passwordHash = this.loginForm.value.password;

      this.authService.login(this.authRequeset).subscribe({
        next: () => {
          this.isSubmitting.set(false);
          this.router.navigate(['/home']);
        },
        error: (error) => {
          this.isSubmitting.set(false);

          if (error.status === 0) {
            this.errorMessage.set('Unable to connect to server. Please check your connection.');
          } else if (error.error?.Message) {
            this.errorMessage.set(error.error.Message);
            console.log(this.errorMessage());
          } else {
            this.errorMessage.set('An unexpected error occurred. Please try again.');
          }
        }
      });

    } else {
      this.loginForm.markAllAsTouched();
    }

    console.log('Outside subscription: ' + this.errorMessage());
  }
}

import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthService } from '../auth.service';
import { Error } from '../../core/models/response';

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {

  constructor(private _auth: AuthService, private _router: Router, route: ActivatedRoute) {
    // The API redirects here with ?error=sso when Google sign-in fails.
    if (route.snapshot.queryParamMap.get('error') === 'sso') {
      this.error.set(['Google sign-in failed. Please try again.']);
    }
  }

  email = signal<string>('');
  password = signal<string>('');

  loading = signal<boolean>(false);
  error = signal<string[]>([]);

  submit(): void {
    const email = this.email().trim();
    const password = this.password();

    this.error.set([]);

    if (!email) {
      this.error.set([...this.error(), 'Email is required.']);
      return;
    }
    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      this.error.set([...this.error(), 'Enter a valid email address.']);
      return;
    }
    if (!password) {
      this.error.set([...this.error(), 'Password is required.']);
      return;
    }

    this.loading.set(true);
    this._auth.login({ email, password }).subscribe({
      next: (user) => {
        this.loading.set(false);
        if (!user) {
          // The login call succeeded but /auth/me did not see the session cookie.
          this.error.set(['Signed in, but the session could not be confirmed. Please try again.']);
          return;
        }
        this._router.navigate(['/revenues']);
      },
      error: (errors: Error[]) => {
        this.error.set(errors.map(err=> err.description));
        this.loading.set(false);
      },
    });
  }

  loginWithGoogle(): void {
    this._auth.loginWithGoogle();
  }
}

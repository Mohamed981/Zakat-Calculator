import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../auth.service';
import { Error, Response } from '../../core/models/response';

@Component({
  selector: 'app-register',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent {

  constructor(private _auth: AuthService, private _router: Router) {}

  firstName = signal<string>('');
  lastName = signal<string>('');
  email = signal<string>('');
  password = signal<string>('');
  confirmPassword = signal<string>('');

  loading = signal<boolean>(false);
  error = signal<string[]>([]);

  submit(): void {
    const firstName = this.firstName().trim();
    const lastName = this.lastName().trim();
    const email = this.email().trim();
    const password = this.password();
    const confirmPassword = this.confirmPassword();

    this.error.set([]);

    if (!firstName) {
      this.error.set([...this.error(), 'First name is required.']);
      return;
    }
    if (!lastName) {
      this.error.set([...this.error(), 'Last name is required.']);
      return;
    }
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
    if (password.length < 6) {
      this.error.set([...this.error(), 'Password must be at least 6 characters.']);
      return;
    }
    if (password !== confirmPassword) {
      this.error.set([...this.error(), 'Passwords do not match.']);
      return;
    }

    this.loading.set(true);
    this._auth.register({ email, firstName, lastName, password }).subscribe({
      next: () => {
        this.loading.set(false);
        this._router.navigate(['/login']);
      },
      error: (errors: Error[]) => {
        this.error.set(errors.map((err: Error) => err.description));
        this.loading.set(false);
      },
    });
  }
}

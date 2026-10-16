import { Component, signal } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from './auth/auth.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  protected readonly title = signal('Angular-21');

  constructor(protected auth: AuthService, private _router: Router) {}

  logout(): void {
    this.auth.logout().subscribe(() => this._router.navigate(['/login']));
  }
}

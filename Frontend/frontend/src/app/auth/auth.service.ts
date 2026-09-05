import { Inject, Injectable, PLATFORM_ID, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Observable, tap } from 'rxjs';
import { ApiService } from '../core/services/api.service';
import { Response } from '../core/models/response';
import { LoginPayload, LoginResult } from '../core/models/login';
import { RegisterPayload, RegisterResult } from '../core/models/register';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class AuthService {

  private readonly isBrowser: boolean;

  constructor(
    private _api: ApiService,
    @Inject(PLATFORM_ID) platformId: Object,
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
    if (this.isBrowser) {
      this.token.set(localStorage.getItem(this.tokenKey));
    }
  }

  private readonly resource = 'auth/api/google';
  private readonly registerResource = 'auth/register';
  private readonly tokenKey = 'accessToken';

  /** Current access token, kept in sync with localStorage. */
  readonly token = signal<string | null>(null);

  /** POST /api/auth/login — stores the returned token on success. */
  login(payload: LoginPayload): Observable<Response<string>> {
    return this._api.post<Response<string>>(this.resource, payload).pipe(
      tap((res) => {
        const token = res.results;
        if (token) {
          if (this.isBrowser) {
            localStorage.setItem(this.tokenKey, token);
          }
          this.token.set(token);
        }
      })
    );
  }

  /** POST /api/auth/register — creates a new user account. */
  register(payload: RegisterPayload): Observable<Response<RegisterResult>> {
    return this._api.post<Response<RegisterResult>>(this.registerResource, payload);
  }

  /** Starts backend-driven Google SSO by navigating the browser to the API's challenge endpoint. */
  loginWithGoogle(): void {
    if (this.isBrowser) {
      window.location.href = `${environment.apiBaseUrl + this.resource}`;
    }
  }

  /** Stores a token handed back from the SSO callback (via URL fragment). */
  setToken(token: string): void {
    if (this.isBrowser) {
      localStorage.setItem(this.tokenKey, token);
    }
    this.token.set(token);
  }

  logout(): void {
    if (this.isBrowser) {
      localStorage.removeItem(this.tokenKey);
    }
    this.token.set(null);
  }

  isAuthenticated(): boolean {
    return this.token() !== null;
  }
}

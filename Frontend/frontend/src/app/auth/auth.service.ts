import { Inject, Injectable, PLATFORM_ID, signal } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Observable, catchError, map, of, switchMap, tap } from 'rxjs';
import { ApiService } from '../core/services/api.service';
import { Response } from '../core/models/response';
import { CurrentUser, LoginPayload } from '../core/models/login';
import { RegisterPayload, RegisterResult } from '../core/models/register';
import { environment } from '../../environments/environment';

/**
 * The session lives in the backend's HttpOnly ACCESS_TOKEN cookie, which the
 * browser sends automatically (same-origin via the /api proxy). The frontend
 * never sees the token; it learns who is signed in from GET /api/auth/me.
 */
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
  }

  private readonly loginResource = 'auth/login';
  private readonly googleResource = 'auth/google';
  private readonly registerResource = 'auth/register';
  private readonly meResource = 'auth/me';
  private readonly logoutResource = 'auth/logout';

  /** Signed-in user, or null when signed out or not yet checked. */
  readonly user = signal<CurrentUser | null>(null);

  /** POST /api/auth/login — the backend sets the session cookie; then load the user. */
  login(payload: LoginPayload): Observable<CurrentUser | null> {
    return this._api.post<Response<string>>(this.loginResource, payload).pipe(
      switchMap(() => this.loadCurrentUser())
    );
  }

  /** POST /api/auth/register — creates a new user account. */
  register(payload: RegisterPayload): Observable<Response<RegisterResult>> {
    return this._api.post<Response<RegisterResult>>(this.registerResource, payload);
  }

  /** Starts backend-driven Google SSO by navigating the browser to the API's challenge endpoint. */
  loginWithGoogle(): void {
    if (this.isBrowser) {
      window.location.href = `${environment.apiBaseUrl}/${this.googleResource}`;
    }
  }

  /** GET /api/auth/me — resolves to the signed-in user, or null if the cookie is missing or expired. */
  loadCurrentUser(): Observable<CurrentUser | null> {
    return this._api.get<Response<CurrentUser>>(this.meResource).pipe(
      map((res) => res.results ?? null),
      catchError(() => of(null)),
      tap((user) => this.user.set(user))
    );
  }

  /** POST /api/auth/logout — the backend clears the HttpOnly cookie. */
  logout(): Observable<void> {
    return this._api.post<void>(this.logoutResource, null).pipe(
      catchError(() => of(undefined)),
      tap(() => this.clearSession())
    );
  }

  /** Forgets the signed-in user locally, e.g. after the API reports the session expired. */
  clearSession(): void {
    this.user.set(null);
  }

  isAuthenticated(): boolean {
    return this.user() !== null;
  }
}

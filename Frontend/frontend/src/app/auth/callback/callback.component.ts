import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

/**
 * Landing target for backend-driven SSO. The API redirects here with the app JWT
 * in the URL fragment (#token=...); we persist it and forward to the app.
 */
@Component({
  selector: 'app-auth-callback',
  imports: [],
  template: `<p class="callback">Signing you in…</p>`,
})
export class CallbackComponent implements OnInit {

  private readonly isBrowser: boolean;

  constructor(
    private _auth: AuthService,
    private _router: Router,
    @Inject(PLATFORM_ID) platformId: Object,
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit(): void {
    if (!this.isBrowser) {
      return;
    }

    const fragment = window.location.hash.startsWith('#')
      ? window.location.hash.slice(1)
      : window.location.hash;
    const token = new URLSearchParams(fragment).get('token');

    if (token) {
      this._auth.setToken(token);
      this._router.navigate(['/revenues']);
    } else {
      this._router.navigate(['/login']);
    }
  }
}

import { Component, Inject, OnInit, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';

/**
 * Landing target for backend-driven SSO. The API has already set the session
 * cookie before redirecting here; confirm it via /api/auth/me and forward to the app.
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

    this._auth.loadCurrentUser().subscribe((user) => {
      this._router.navigate([user ? '/revenues' : '/login']);
    });
  }
}

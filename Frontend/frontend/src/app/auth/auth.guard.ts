import { PLATFORM_ID, inject } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { CanActivateFn, Router } from '@angular/router';
import { map } from 'rxjs';
import { AuthService } from './auth.service';

/** Redirects unauthenticated users to the login page. */
export const authGuard: CanActivateFn = () => {
  // Routes are prerendered at build time, where there is no session cookie;
  // the check runs again in the browser on the initial navigation.
  if (!isPlatformBrowser(inject(PLATFORM_ID))) {
    return true;
  }

  const auth = inject(AuthService);
  const router = inject(Router);

  if (auth.isAuthenticated()) {
    return true;
  }

  return auth.loadCurrentUser().pipe(
    map((user) => user !== null || router.createUrlTree(['/login']))
  );
};

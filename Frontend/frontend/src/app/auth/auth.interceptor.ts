import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { AuthService } from './auth.service';

/** Endpoints whose 401s are handled by their callers rather than treated as an expired session. */
const SESSION_CHECK_URLS = ['/auth/me', '/auth/login'];

/**
 * When the ACCESS_TOKEN cookie expires, the API starts answering 401.
 * Clear the cached user and send the browser to the login page.
 */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      const isSessionCheck = SESSION_CHECK_URLS.some((url) => req.url.endsWith(url));
      if (err.status === 401 && !isSessionCheck) {
        auth.clearSession();
        router.navigate(['/login']);
      }
      return throwError(() => err);
    })
  );
};

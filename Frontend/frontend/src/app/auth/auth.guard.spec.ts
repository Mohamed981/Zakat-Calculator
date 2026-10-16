import { TestBed } from '@angular/core/testing';
import { PLATFORM_ID } from '@angular/core';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree, provideRouter } from '@angular/router';
import { Observable, firstValueFrom, isObservable } from 'rxjs';
import { authGuard } from './auth.guard';
import { AuthService } from './auth.service';

describe('authGuard', () => {
  let http: HttpTestingController;

  function setup(platform: 'browser' | 'server' = 'browser'): void {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([]),
        { provide: PLATFORM_ID, useValue: platform },
      ],
    });
    http = TestBed.inject(HttpTestingController);
  }

  function runGuard() {
    return TestBed.runInInjectionContext(() =>
      authGuard({} as ActivatedRouteSnapshot, {} as RouterStateSnapshot)
    );
  }

  afterEach(() => http.verify());

  it('allows prerendering on the server without calling the API', () => {
    setup('server');
    expect(runGuard()).toBeTrue();
  });

  it('allows a user that is already loaded', () => {
    setup();
    TestBed.inject(AuthService).user.set({ userId: 1, email: null, name: null });
    expect(runGuard()).toBeTrue();
  });

  it('asks /auth/me when the user is unknown and allows a valid session', async () => {
    setup();
    const result = runGuard();
    expect(isObservable(result)).toBeTrue();

    const pending = firstValueFrom(result as Observable<boolean | UrlTree>);
    http.expectOne('/api/auth/me').flush({ results: { userId: 1, email: null, name: null } });

    expect(await pending).toBeTrue();
  });

  it('redirects to /login when there is no session', async () => {
    setup();
    const pending = firstValueFrom(runGuard() as Observable<boolean | UrlTree>);
    http.expectOne('/api/auth/me').flush(null, { status: 401, statusText: 'Unauthorized' });

    const router = TestBed.inject(Router);
    expect(await pending).toEqual(router.createUrlTree(['/login']));
  });
});

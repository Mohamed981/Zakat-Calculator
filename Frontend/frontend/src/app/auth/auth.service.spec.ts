import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideRouter } from '@angular/router';
import { AuthService } from './auth.service';
import { CurrentUser } from '../core/models/login';

describe('AuthService', () => {
  let service: AuthService;
  let http: HttpTestingController;

  const user: CurrentUser = { userId: 1, email: 'a@b.com', name: 'A' };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    service = TestBed.inject(AuthService);
    http = TestBed.inject(HttpTestingController);
  });

  afterEach(() => http.verify());

  it('loadCurrentUser stores the user returned by /auth/me', () => {
    let result: CurrentUser | null | undefined;
    service.loadCurrentUser().subscribe((u) => (result = u));

    http.expectOne('/api/auth/me').flush({ results: user, errors: [], messages: [] });

    expect(result).toEqual(user);
    expect(service.user()).toEqual(user);
    expect(service.isAuthenticated()).toBeTrue();
  });

  it('loadCurrentUser resolves to null when the session is missing', () => {
    service.user.set(user);
    let result: CurrentUser | null | undefined;
    service.loadCurrentUser().subscribe((u) => (result = u));

    http.expectOne('/api/auth/me').flush(null, { status: 401, statusText: 'Unauthorized' });

    expect(result).toBeNull();
    expect(service.isAuthenticated()).toBeFalse();
  });

  it('login posts the credentials and then loads the user', () => {
    let result: CurrentUser | null | undefined;
    service.login({ email: 'a@b.com', password: 'pw' }).subscribe((u) => (result = u));

    const login = http.expectOne('/api/auth/login');
    expect(login.request.method).toBe('POST');
    expect(login.request.body).toEqual({ email: 'a@b.com', password: 'pw' });
    login.flush({ results: 'jwt', errors: [], messages: [] });

    http.expectOne('/api/auth/me').flush({ results: user, errors: [], messages: [] });

    expect(result).toEqual(user);
  });

  it('login errors with the API errors and skips /auth/me on bad credentials', () => {
    const errors = [{ code: 'Users.NotFoundByEmail', description: 'nope' }];
    let received: unknown;
    service.login({ email: 'a@b.com', password: 'bad' }).subscribe({ error: (e) => (received = e) });

    http.expectOne('/api/auth/login').flush(errors, { status: 400, statusText: 'Bad Request' });

    expect(received).toEqual(errors);
    http.expectNone('/api/auth/me');
  });

  it('logout clears the user even if the request fails', () => {
    service.user.set(user);
    service.logout().subscribe();

    const req = http.expectOne('/api/auth/logout');
    expect(req.request.method).toBe('POST');
    req.flush(null, { status: 500, statusText: 'Server Error' });

    expect(service.user()).toBeNull();
  });
});

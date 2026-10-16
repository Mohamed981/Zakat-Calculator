import { TestBed } from '@angular/core/testing';
import { HttpClient, provideHttpClient, withInterceptors } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { Router, provideRouter } from '@angular/router';
import { authInterceptor } from './auth.interceptor';
import { AuthService } from './auth.service';

describe('authInterceptor', () => {
  let client: HttpClient;
  let http: HttpTestingController;
  let auth: AuthService;
  let navigate: jasmine.Spy;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(withInterceptors([authInterceptor])),
        provideHttpClientTesting(),
        provideRouter([]),
      ],
    });
    client = TestBed.inject(HttpClient);
    http = TestBed.inject(HttpTestingController);
    auth = TestBed.inject(AuthService);
    navigate = spyOn(TestBed.inject(Router), 'navigate').and.resolveTo(true);
    auth.user.set({ userId: 1, email: null, name: null });
  });

  afterEach(() => http.verify());

  it('clears the session and redirects to /login on a 401', () => {
    let failed = false;
    client.get('/api/revenues').subscribe({ error: () => (failed = true) });

    http.expectOne('/api/revenues').flush(null, { status: 401, statusText: 'Unauthorized' });

    expect(failed).toBeTrue();
    expect(auth.user()).toBeNull();
    expect(navigate).toHaveBeenCalledWith(['/login']);
  });

  it('leaves other errors alone', () => {
    client.get('/api/revenues').subscribe({ error: () => {} });

    http.expectOne('/api/revenues').flush(null, { status: 500, statusText: 'Server Error' });

    expect(auth.user()).not.toBeNull();
    expect(navigate).not.toHaveBeenCalled();
  });

  for (const url of ['/api/auth/me', '/api/auth/login']) {
    it(`does not redirect on a 401 from ${url}`, () => {
      client.get(url).subscribe({ error: () => {} });

      http.expectOne(url).flush(null, { status: 401, statusText: 'Unauthorized' });

      expect(navigate).not.toHaveBeenCalled();
    });
  }
});

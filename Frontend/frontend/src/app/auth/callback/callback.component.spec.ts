import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { Router, provideRouter } from '@angular/router';
import { CallbackComponent } from './callback.component';

describe('CallbackComponent', () => {
  let http: HttpTestingController;
  let navigate: jasmine.Spy;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [CallbackComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    });
    http = TestBed.inject(HttpTestingController);
    navigate = spyOn(TestBed.inject(Router), 'navigate').and.resolveTo(true);
    TestBed.createComponent(CallbackComponent).detectChanges();
  });

  afterEach(() => http.verify());

  it('forwards to /revenues once the session is confirmed', () => {
    http.expectOne('/api/auth/me').flush({ results: { userId: 1, email: null, name: null } });
    expect(navigate).toHaveBeenCalledWith(['/revenues']);
  });

  it('goes back to /login when the SSO cookie was not set', () => {
    http.expectOne('/api/auth/me').flush(null, { status: 401, statusText: 'Unauthorized' });
    expect(navigate).toHaveBeenCalledWith(['/login']);
  });
});

import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { Router, provideRouter } from '@angular/router';
import { AppComponent } from './app.component';
import { AuthService } from './auth/auth.service';

describe('AppComponent', () => {
  let http: HttpTestingController;
  let auth: AuthService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppComponent],
      providers: [provideHttpClient(), provideHttpClientTesting(), provideRouter([])],
    }).compileComponents();
    http = TestBed.inject(HttpTestingController);
    auth = TestBed.inject(AuthService);
  });

  afterEach(() => http.verify());

  it('hides the session bar when signed out', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    expect(fixture.nativeElement.querySelector('.session-bar')).toBeNull();
  });

  it('shows the signed-in user and signs out', () => {
    const navigate = spyOn(TestBed.inject(Router), 'navigate').and.resolveTo(true);
    auth.user.set({ userId: 1, email: 'a@b.com', name: null });
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();

    const el = fixture.nativeElement as HTMLElement;
    expect(el.querySelector('.session-bar__user')?.textContent).toContain('a@b.com');

    el.querySelector<HTMLButtonElement>('.session-bar__logout')!.click();
    http.expectOne('/api/auth/logout').flush(null);
    fixture.detectChanges();

    expect(auth.user()).toBeNull();
    expect(navigate).toHaveBeenCalledWith(['/login']);
    expect(el.querySelector('.session-bar')).toBeNull();
  });
});

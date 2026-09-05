import { Routes } from '@angular/router';
import { CurrenciesComponent } from './currencies/currencies.component';
import { AdminComponent } from './admin/admin.component';
import { RevenuesComponent } from './revenues/revenues.component';
import { LoginComponent } from './auth/login/login.component';
import { RegisterComponent } from './auth/register/register.component';
import { CallbackComponent } from './auth/callback/callback.component';
import { authGuard } from './auth/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'currencies', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'auth/callback', component: CallbackComponent },
  { path: 'currencies', component: CurrenciesComponent, canActivate: [authGuard] },
  { path: 'revenues', component: RevenuesComponent},
  { path: 'admin', component: AdminComponent, canActivate: [authGuard] },
];

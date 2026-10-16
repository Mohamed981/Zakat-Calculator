# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Repository layout

- `Backend/` — .NET 10 solution `ZakatMonitoring.slnx` (Clean Architecture + CQRS with MediatR, EF Core on SQL Server, Redis cache, .NET Aspire orchestration).
- `Frontend/frontend/` — Angular 21 standalone app with SSR (`@angular/ssr` + Express in `src/server.ts`).
- `Frontend/frontend/test.js` is a vendored, minified `qrcode` library, not a test file.
- `Code/App/ZakatApp/` contains only committed `obj/` build output from an old .NET 7 Android app. It has no source; ignore it.

Active development happens on the `c#v3` branch. `.github/workflows/main.yml` only runs on pushes to `c#v3`, and it only echoes text and lists files. There is no real CI.

## Commands

Backend (from `Backend/`):

```
dotnet build ZakatMonitoring.slnx
dotnet run --project Aspire.AppHost          # full stack: Redis + API + Angular dev server, with the Aspire dashboard
dotnet run --project ZakatMonitoring         # API only (http://localhost:5080, https://localhost:7080); needs Redis + SQL Server reachable
dotnet ef migrations add <Name> --project Infrastructure --startup-project ZakatMonitoring
```

There are no backend test projects. In Development the API serves OpenAPI plus the Scalar UI at `/scalar`.

Required configuration (`appsettings.json` leaves it blank, so use user-secrets on the API project):
- `ConnectionStrings:dbcontext` for SQL Server.
- `Jwt:Secret`, `Jwt:Issuer`, `Jwt:Audience`, `Jwt:ExpirationInMinutes`.
- `Authentication:Google:ClientId` / `ClientSecret`.
- `Frontend:BaseUrl`, where the Google callback redirects after login.
- The Redis connection `cache`, which Aspire injects.

Frontend (from `Frontend/frontend/`):

```
npm install
npx ng serve                    # dev server; `npm start` expects a PORT env var (set by Aspire)
npm run build
npm test                        # Karma + Jasmine
npx ng test --include src/app/path/to/file.spec.ts
```

## Backend architecture

Project dependencies run `API (ZakatMonitoring/, RootNamespace API)` → `Infrastructure` → `Application` → `Domain` → `SharedKernel`. The API also references `Aspire.ServiceDeafaults` (the misspelling is the real project name). `SharedKernel` is not listed in the `.slnx`; it builds only through Domain's project reference.

- **Request flow:** controllers in `ZakatMonitoring/Controllers` map a `Requests/*Request` to a MediatR command or query and call `mediator.Send`. Handlers live in `Application/<Feature>/{Commands,Queries}` and are `internal sealed`. MediatR handlers and FluentValidation validators are registered by scanning the assembly that contains `Application.IAssemblyMarker`.
- **Errors and validation:** `ValidationBehavior` runs the validators and throws `ValidationException`. `GlobalExceptionHandler` maps exceptions to status codes: 400 for validation, 404 for `NotFoundException`, 500 otherwise. Errors are returned in a `Result<T>` (`Results`, `Errors: List<SharedKernel.Error>`, `Messages`).
  - Business failures (e.g. `Domain/Users/UserErrors`) go into `Result.Errors` instead of being thrown. `AuthController` turns those into `BadRequest`.
- **Data access:** handlers depend on `IAppDbContext`, which `Infrastructure/Data/AppDbContext` implements.
  - The schema was reverse-engineered from a database originally created by a Java/Hibernate backend. That is why the tables and columns use snake_case and the FK/PK names are Hibernate-generated.
  - `Infrastructure/Migrations` holds only a model snapshot, with no migration files.
- **Caching:** `ICacheService` is backed by `RedisCacheService` over `IDistributedCache`. The `GET /api/currencies` result is cached under `currencies:all` (`CacheKeys.Currencies`), and `UpdateCurrency` invalidates that key.
- **Auth (cookie-only session):**
  - `TokenProvider` issues a JWT with the user id in `sub`. `AuthController` stores it in the `ACCESS_TOKEN` HttpOnly cookie on both `POST /api/auth/login` and the Google callback. The JWT bearer handler reads that cookie and falls back to the `Authorization` header.
  - `GET /api/auth/me` (`[Authorize]`) returns the current user. `POST /api/auth/logout` deletes the cookie.
  - Google SSO goes `GET /api/auth/google` → `/api/auth/google/callback`, which redirects to `{Frontend:BaseUrl}/auth/callback`. Failures inside the Google handshake never reach the controller: `OnRemoteFailure` in `Program.cs` redirects them to `{Frontend:BaseUrl}/login?error=sso`.
  - `IUserContext.UserId` reads the current user from the claims. Only `RevenuesController` and `/auth/me` have `[Authorize]`.

## Zakat domain logic

All monetary fields on the entities are **strings** and are parsed with `double.Parse` in the handlers.

- Everything is normalized to **21K gold**. Each `Currency.Rates` holds that currency's rate. The row with `Code == "21K"` is the reference, and code `xau` is hidden from the currency list.
- Creating or updating a revenue computes `zakatIn21K = rate(currency) / rate(21K) * value * 0.025` and adds it to `User.TotalZakatIn21k`.
- `ZakatConstant.ZAKAT_THRESHOLD` (2.125) is the nisab, in 21K units. The handlers hard-code `2.125` instead of using the constant.
- When the total first crosses the nisab, `User.ZakatDeadLine` is set to today's Hijri date (`ZakatUtil`, `ar-SA` culture). Revenues whose `ZakatRemaining == "0"` are then set to 2.5% of their value. If an update drops the total, the deadline is cleared and every `ZakatRemaining` is reset.
- `CreateTransaction` (a zakat payment) converts the paid amount into the revenue's currency, moves it from `ZakatRemaining` to `ZakatPaid`, and adds to `User.TotalPaidZakatIn21k`.
- `CreateCurrenciesCommandHandler` is still `NotImplementedException`. Currency rates are maintained through `PUT /api/currencies/{id}`.

## Frontend architecture

- **API access:** all backend calls go through `core/services/ApiService`, which prefixes `environment.apiBaseUrl` (`/api`), then `RequestService`, which adds `Accept-Language`. The frontend never handles the JWT: the browser sends the HttpOnly cookie because `/api` is same-origin through the proxy.
- **Auth state:** `AuthService.user` is filled from `GET /api/auth/me`. `authGuard` calls it when the user is unknown, and skips the check on the server so that prerendering works. `CallbackComponent` confirms the session after Google SSO.
  - `auth/auth.interceptor.ts` handles any 401 by clearing the session and navigating to `/login`. It skips `/auth/me` and `/auth/login`, whose callers handle their own 401s. Add any new session-probe endpoint to `SESSION_CHECK_URLS`.
  - In dev, `proxy.conf.js` forwards `/api` to the API URL that Aspire injects (`services__api__https__0` / `services__api__http__0`). Without Aspire it falls back to `https://localhost:7080`, the API's `launchSettings.json` https port.
- **SSR:** the app is server-rendered, so guard any `localStorage`/`window` access with `isPlatformBrowser`, as `AuthService` and `RequestService` do.
- **State:** components use signals. `RevenueService` keeps a client-side `created` list, and `AdminService` holds manually entered rates in memory only.
- **Routes** (`app.routes.ts`): `currencies`, `revenues` and `admin` use `authGuard`; `login`, `register` and `auth/callback` are public.
- **Prerendering:** all routes are prerendered (`app.routes.server.ts`). `ng build` currently fails with a timeout while prerendering `/revenues`.
- **External rates:** `currencies/currency.service.ts` gets FX rates directly from api.frankfurter.app. `backend-currency.service.ts` gets the currency list from the backend.
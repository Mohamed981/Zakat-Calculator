using System.Security.Claims;
using API.Requests;
using Application.Users.ExternalLogin;
using Application.Users.GetCurrentUser;
using Application.Users.Login;
using Application.Users.Register.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator, IConfiguration configuration) : ControllerBase
{
    // The JWT bearer handler reads the token from this cookie
    // (see Program.cs JwtBearerEvents.OnMessageReceived).
    private const string AccessTokenCookie = "ACCESS_TOKEN";

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var command = new LoginUserCommand(request.Email, request.Password);
        var result = await mediator.Send(command);
        if (result.Errors.Any())
        {
            return BadRequest(result.Errors);
        }

        SetAccessTokenCookie(result.Results);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var command = new RegisterUserCommand(request.Email, request.FirstName, request.LastName, request.Password);
        var result = await mediator.Send(command);
        if (result.Errors.Any())
        {
            return BadRequest(result.Errors);
        }
        SetAccessTokenCookie(result.Results);
        return Ok(result);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var result = await mediator.Send(new GetCurrentUserQuery());
        if (result.Errors.Any())
        {
            return Unauthorized(result.Errors);
        }

        return Ok(result);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete(AccessTokenCookie, BuildAccessTokenCookieOptions());
        return NoContent();
    }

    [HttpGet("google")]
    public IActionResult GoogleLogin()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = "/api/auth/google/callback"
        };
        return Challenge(properties, "Google");
    }

    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleCallback()
    {
        var authResult = await HttpContext.AuthenticateAsync("Google");

        // The Google handler only uses the external "Cookies" scheme to carry the
        // principal across the redirect; the app session is the ACCESS_TOKEN cookie.
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!authResult.Succeeded)
        {
            return RedirectToFrontendLogin();
        }

        ClaimsPrincipal principal = authResult.Principal;
        string? externalId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(externalId))
        {
            return RedirectToFrontendLogin();
        }

        string email = principal.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        string name = principal.FindFirstValue(ClaimTypes.Name) ?? email;

        var command = new ExternalLoginCommand("Google", externalId, email, name);
        var result = await mediator.Send(command);
        if (result.Errors.Any())
        {
            return RedirectToFrontendLogin();
        }

        SetAccessTokenCookie(result.Results);

        // The frontend's /auth/callback page confirms the session via GET /api/auth/me.
        return Redirect($"{FrontendBaseUrl}/auth/callback");
    }

    private string FrontendBaseUrl => (configuration["Frontend:BaseUrl"] ?? string.Empty).TrimEnd('/');

    // Google sign-in happens in a full-page redirect, so failures go back to the
    // login page (which shows a message for ?error=sso) instead of returning JSON.
    private RedirectResult RedirectToFrontendLogin() => Redirect($"{FrontendBaseUrl}/login?error=sso");

    private void SetAccessTokenCookie(string token)
    {
        var options = BuildAccessTokenCookieOptions();
        options.Expires = DateTimeOffset.UtcNow.AddMinutes(
            configuration.GetValue<int>("Jwt:ExpirationInMinutes"));
        Response.Cookies.Append(AccessTokenCookie, token, options);
    }

    private static CookieOptions BuildAccessTokenCookieOptions() => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Lax,
        Path = "/"
    };
}

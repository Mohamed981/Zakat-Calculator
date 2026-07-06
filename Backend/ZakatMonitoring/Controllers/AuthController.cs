using System.Security.Claims;
using API.Requests;
using Application.Users.ExternalLogin;
using Application.Users.Login;
using Application.Users.Register.Commands;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator, IConfiguration configuration) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        var command = new LoginUserCommand(request.Email, request.Password);
        var result = await mediator.Send(command);
        if (result.Errors.Any())
        {
            return BadRequest(result.Errors);
        }

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

        return Ok(result);
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
        if (!authResult.Succeeded)
        {
            return Unauthorized();
        }

        ClaimsPrincipal principal = authResult.Principal;
        string externalId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        string email = principal.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
        string name = principal.FindFirstValue(ClaimTypes.Name) ?? email;

        var command = new ExternalLoginCommand("Google", externalId, email, name);
        var result = await mediator.Send(command);
        if (result.Errors.Any())
        {
            return BadRequest(result.Errors);
        }

        // The JWT bearer handler reads the token from the ACCESS_TOKEN cookie
        // (see Program.cs JwtBearerEvents.OnMessageReceived), so set it here
        // before redirecting to the frontend, otherwise the SSO login would
        // not authenticate any subsequent request.
        Response.Cookies.Append("ACCESS_TOKEN", result.Results, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddMinutes(
                configuration.GetValue<int>("Jwt:ExpirationInMinutes"))
        });

        string frontendBaseUrl = configuration["Frontend:BaseUrl"] ?? "/";
        return Redirect(frontendBaseUrl);
    }
}

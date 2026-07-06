using System;
using Application.Abstractions.Authentication;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authentication;

internal sealed class UserContext : IUserContext
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public int UserId => (_httpContextAccessor.HttpContext ?? throw new ApplicationException("User context is unavailable")).User.GetUserId();

	public UserContext(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}
}

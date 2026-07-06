using System;
using System.Security.Claims;

namespace Infrastructure.Authentication;

internal static class ClaimsPrincipalExtensions
{
	public static int GetUserId(this ClaimsPrincipal? principal)
	{
		string s = principal?.FindFirstValue("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
		if (!int.TryParse(s, out var result))
		{
			throw new ApplicationException("User id is unavailable");
		}
		return result;
	}
}

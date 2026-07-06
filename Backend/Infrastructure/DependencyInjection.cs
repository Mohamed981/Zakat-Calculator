using Application.Abstractions.Authentication;
using Application.Common.Interfaces;
using Infrastructure.Authentication;
using Infrastructure.Caching;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
	{
		services.AddScoped<IUserContext, UserContext>();
		services.AddScoped<ICacheService, RedisCacheService>();
		services.AddSingleton<IPasswordHasher, PasswordHasher>();
		services.AddSingleton<ITokenProvider, TokenProvider>();
		services.AddHttpContextAccessor();
		return services;
	}
}

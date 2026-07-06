using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.Authentication;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.ExternalLogin;

internal sealed class ExternalLoginCommandHandler(IAppDbContext context, ITokenProvider tokenProvider) : IRequestHandler<ExternalLoginCommand, Result<string>>
{
	public async Task<Result<string>> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
	{
		User user = await context.Users.SingleOrDefaultAsync((User u) => u.Provider == request.Provider && u.ExternalId == request.ExternalId, cancellationToken);
		if (user == null)
		{
			user = await context.Users.SingleOrDefaultAsync((User u) => u.Email == request.Email, cancellationToken);
			if (user != null)
			{
				user.Provider = request.Provider;
				user.ExternalId = request.ExternalId;
				await context.SaveChangesAsync(cancellationToken);
			}
		}
		if (user == null)
		{
			user = new User
			{
				Email = request.Email,
				Name = request.Name,
				Provider = request.Provider,
				ExternalId = request.ExternalId
			};
			context.Users.Add(user);
			await context.SaveChangesAsync(cancellationToken);
		}
		string token = tokenProvider.Create(user);
		return new Result<string>
		{
			Results = token
		};
	}
}

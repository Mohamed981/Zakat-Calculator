using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.Authentication;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Login;

internal sealed class LoginUserCommandHandler(IAppDbContext context, IPasswordHasher passwordHasher, ITokenProvider tokenProvider) : IRequestHandler<LoginUserCommand, Result<string>>
{
	public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
	{
		User user = await context.Users.AsNoTracking().SingleOrDefaultAsync((User u) => u.Email == request.Email, cancellationToken);
		if (user == null)
		{
			return await Task.FromResult(new Result<string>
			{
				Errors = { UserErrors.NotFoundByEmail }
			});
		}
		if (!passwordHasher.Verify(request.Password, user.Password))
		{
			return await Task.FromResult(new Result<string>
			{
				Errors = { UserErrors.NotFoundByEmail }
			});
		}
		string token = tokenProvider.Create(user);
		return await Task.FromResult(new Result<string>
		{
			Results = token
		});
	}
}

using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.Authentication;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Register.Commands;

internal sealed class RegisterUserCommandHandler(IAppDbContext context, IPasswordHasher passwordHasher, ITokenProvider tokenProvider) : IRequestHandler<RegisterUserCommand, Result<string>>
{
	public async Task<Result<string>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		if (await context.Users.AnyAsync((User u) => u.Email == request.Email, cancellationToken))
		{
			return await Task.FromResult(new Result<string>
			{
				Errors = { UserErrors.EmailNotUnique }
			});
		}
		User user = new User
		{
			Email = request.Email,
			Name = request.FirstName + " " + request.LastName,
			Password = passwordHasher.Hash(request.Password)
		};
		context.Users.Add(user);
		await context.SaveChangesAsync(cancellationToken);
		string token = tokenProvider.Create(user);
		return await Task.FromResult(new Result<string>
		{
			Results = token
		});
	}
}

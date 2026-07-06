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

internal sealed class RegisterUserCommandHandler(IAppDbContext context, IPasswordHasher passwordHasher) : IRequestHandler<RegisterUserCommand, Result<int>>
{
	public async Task<Result<int>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		if (await context.Users.AnyAsync((User u) => u.Email == request.Email, cancellationToken))
		{
			return await Task.FromResult(new Result<int>
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
		return await Task.FromResult(new Result<int>
		{
			Results = user.UserId
		});
	}
}

using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Users.Commands;

internal sealed class CreateUserCommandHandler(IAppDbContext context) : IRequestHandler<CreateUserCommand, string>
{
	public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
	{
		context.Users.Add(new User
		{
			Name = request.Name
		});
		await context.SaveChangesAsync(cancellationToken);
		return request.Name + " Added";
	}
}

using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.Authentication;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.GetCurrentUser;

internal sealed class GetCurrentUserQueryHandler(IAppDbContext context, IUserContext userContext) : IRequestHandler<GetCurrentUserQuery, Result<CurrentUserResponse>>
{
	public async Task<Result<CurrentUserResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
	{
		int userId = userContext.UserId;
		CurrentUserResponse? user = await context.Users.AsNoTracking()
			.Where(u => u.UserId == userId)
			.Select(u => new CurrentUserResponse
			{
				UserId = u.UserId,
				Email = u.Email,
				Name = u.Name
			})
			.SingleOrDefaultAsync(cancellationToken);
		if (user == null)
		{
			return new Result<CurrentUserResponse>
			{
				Errors = { UserErrors.NotFound(userId) }
			};
		}
		return new Result<CurrentUserResponse>
		{
			Results = user
		};
	}
}

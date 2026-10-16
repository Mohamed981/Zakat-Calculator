using Application.Common.Models;
using MediatR;

namespace Application.Users.GetCurrentUser;

public record GetCurrentUserQuery : IRequest<Result<CurrentUserResponse>>, IBaseRequest;

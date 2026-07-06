using MediatR;

namespace Application.Users.Commands;

public sealed record CreateUserCommand(string Name) : IRequest<string>, IBaseRequest;

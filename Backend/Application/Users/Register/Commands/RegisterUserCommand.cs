using Application.Common.Models;
using MediatR;

namespace Application.Users.Register.Commands;

public sealed record RegisterUserCommand(string Email, string FirstName, string LastName, string Password) : IRequest<Result<int>>, IBaseRequest;

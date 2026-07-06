using Application.Common.Models;
using MediatR;

namespace Application.Users.ExternalLogin;

public record ExternalLoginCommand(string Provider, string ExternalId, string Email, string Name) : IRequest<Result<string>>, IBaseRequest;

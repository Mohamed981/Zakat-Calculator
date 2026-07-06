using Application.Common.Models;
using MediatR;

namespace Application.Revenues.Commands.DeleteRevenueCommand;

public sealed record DeleteRevenueCommand(string id) : IRequest<Result<string>>, IBaseRequest;

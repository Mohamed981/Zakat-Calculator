using Application.Common.Models;
using MediatR;

namespace Application.Revenues.Commands.CreateRevenueCommand;

public sealed record CreateRevenueCommand(int userId, int currencyId, double value) : IRequest<Result<string>>, IBaseRequest;

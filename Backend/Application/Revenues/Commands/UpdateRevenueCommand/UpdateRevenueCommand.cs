using Application.Common.Models;
using MediatR;

namespace Application.Revenues.Commands.UpdateRevenueCommand;

public sealed record UpdateRevenueCommand(int id, int userId, int currencyId, double value) : IRequest<Result<string>>, IBaseRequest;

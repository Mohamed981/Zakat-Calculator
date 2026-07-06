using Application.Common.Models;
using MediatR;

namespace Application.Transactions.Commands;

public sealed record CreateTransactionCommand(int revenueId, int currencyId, int userId, double value) : IRequest<Result<string>>, IBaseRequest;

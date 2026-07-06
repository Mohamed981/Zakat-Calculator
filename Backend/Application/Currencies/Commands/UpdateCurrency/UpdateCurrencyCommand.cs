using Application.Common.Models;
using MediatR;

namespace Application.Currencies.Commands.UpdateCurrency;

public sealed record UpdateCurrencyCommand(int Id, string Name, string Value) : IRequest<Result<string>>, IBaseRequest;

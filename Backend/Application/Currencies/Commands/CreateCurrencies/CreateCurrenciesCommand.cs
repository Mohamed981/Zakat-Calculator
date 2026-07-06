using Application.Common.Models;
using MediatR;

namespace Application.Currencies.Commands.CreateCurrencies;

public sealed record CreateCurrenciesCommand(string code) : IRequest<Result<string>>
{
}

using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.Currencies.Commands.CreateCurrencies;

internal sealed class CreateCurrenciesCommandHandler(IAppDbContext context) : IRequestHandler<CreateCurrenciesCommand, Result<string>>
{
    public Task<Result<string>> Handle(CreateCurrenciesCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

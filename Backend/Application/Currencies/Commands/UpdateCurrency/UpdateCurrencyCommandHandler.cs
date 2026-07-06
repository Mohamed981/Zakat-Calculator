using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.Currencies.Commands.UpdateCurrency;

internal sealed class UpdateCurrencyCommandHandler(IAppDbContext context, ICacheService cache) : IRequestHandler<UpdateCurrencyCommand, Result<string>>
{
	public async Task<Result<string>> Handle(UpdateCurrencyCommand request, CancellationToken cancellationToken)
	{
		Currency currency = await context.Currencies.FindAsync(request.Id);
		if (currency == null)
		{
			throw new InvalidOperationException("Currency not found");
		}
		currency.Name = request.Name;
		currency.Rates = request.Value;
		await context.SaveChangesAsync(cancellationToken);
		await cache.RemoveAsync("currencies:all", cancellationToken);
		return new Result<string>
		{
			Results = "Currency updated successfully"
		};
	}
}

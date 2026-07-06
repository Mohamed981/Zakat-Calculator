using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.Authentication;
using Application.Common.Interfaces;
using Application.Revenues.Commands.Models;
using Domain.Entities;
using MediatR;

namespace Application.Revenues.Queries;

internal sealed class GetRevenuesQueryHandler(IAppDbContext context, IUserContext userContext) : IRequestHandler<GetRevenuesQuery, List<GetRevenuesResponse>>
{
	public async Task<List<GetRevenuesResponse>> Handle(GetRevenuesQuery request, CancellationToken cancellationToken)
	{
		return await Task.FromResult(context.Revenues.Where((Revenue c) => c.UserId == (int?)userContext.UserId).Join(context.Currencies, (Revenue r) => r.CurrencyId, (Currency c) => c.CurrencyId, (Revenue revenue, Currency currency) => new GetRevenuesResponse
		{
			Id = revenue.RevenueId,
			Name = currency.Name,
			Value = revenue.Value
		}).ToList());
	}
}

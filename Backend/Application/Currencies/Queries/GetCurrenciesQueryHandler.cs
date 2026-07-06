using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Currencies.Queries.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Currencies.Queries;

public sealed class GetCurrenciesQueryHandler(IAppDbContext context, ICacheService cache) : IRequestHandler<GetCurrenciesQuery, List<GetCurrenciesQueryResponse>>
{
	public async Task<List<GetCurrenciesQueryResponse>> Handle(GetCurrenciesQuery request, CancellationToken cancellationToken)
	{
		List<GetCurrenciesQueryResponse> cached = await cache.GetAsync<List<GetCurrenciesQueryResponse>>("currencies:all", cancellationToken);
		if (cached != null)
		{
			return cached;
		}
		List<GetCurrenciesQueryResponse> result = await (from c in context.Currencies
			where c.Code != "xau" && c.Code != null
			select new GetCurrenciesQueryResponse
			{
				CurrencyId = c.CurrencyId,
				Code = c.Code,
				Name = c.Name
			}).ToListAsync(cancellationToken);
		await cache.SetAsync("currencies:all", result, TimeSpan.FromHours(1), cancellationToken);
		return result;
	}
}

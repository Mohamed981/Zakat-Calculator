using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Utils;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Revenues.Commands.CreateRevenueCommand;

internal sealed class CreateRevenueCommandHandler(IAppDbContext context) : IRequestHandler<CreateRevenueCommand, Result<string>>
{
	public async Task<Result<string>> Handle(CreateRevenueCommand request, CancellationToken cancellationToken)
	{
		User user = await context.Users.Include((User u) => u.Revenues).FirstOrDefaultAsync((User u) => u.UserId == request.userId);
		Currency currency = await context.Currencies.FindAsync(request.currencyId);
		string kerat21 = await (from c in context.Currencies
			where c.Code == "21K"
			select c.Rates).FirstOrDefaultAsync();
		double curruncyValue = double.Parse(currency.Rates, NumberStyles.Float);
		double kerat21Value = double.Parse(kerat21, NumberStyles.Float);
		double zakatIn21K = curruncyValue / kerat21Value * request.value * 0.025;
		Revenue newRevenue = new Revenue
		{
			UserId = request.userId,
			CurrencyId = request.currencyId,
			Value = request.value.ToString(),
			ZakatIn21k = zakatIn21K.ToString(),
			ZakatRemaining = zakatIn21K.ToString(),
			ZakatPaid = "0"
		};
		context.Revenues.Add(newRevenue);
		double userZakat = double.Parse(user.TotalZakatIn21k) + zakatIn21K;
		user.TotalZakatIn21k = userZakat.ToString();
		if (userZakat >= 2.125)
		{
			if (user.ZakatDeadLine == null)
			{
				DateTime gregorianDate = DateTime.Now;
				string hijriDate = ZakatUtil.UsingHijrahChronology(gregorianDate);
				user.ZakatDeadLine = hijriDate;
			}
			foreach (Revenue revenue in user.Revenues)
			{
				if (revenue.ZakatRemaining == "0")
				{
					revenue.ZakatRemaining = (double.Parse(revenue.Value ?? "0") * 0.025).ToString();
					context.Revenues.Update(revenue);
				}
			}
		}
		await context.SaveChangesAsync(cancellationToken);
		return await Task.FromResult(new Result<string>
		{
			Results = newRevenue.RevenueId.ToString()
		});
	}
}

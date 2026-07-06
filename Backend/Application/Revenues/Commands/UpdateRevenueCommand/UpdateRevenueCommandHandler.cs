using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Utils;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Revenues.Commands.UpdateRevenueCommand;

internal sealed class UpdateRevenueCommandHandler(IAppDbContext context) : IRequestHandler<UpdateRevenueCommand, Result<string>>
{
	public async Task<Result<string>> Handle(UpdateRevenueCommand request, CancellationToken cancellationToken)
	{
		Revenue revenue = context.Revenues.Find(request.id);
		User user = await context.Users.FindAsync(request.userId);
		Currency currency = await context.Currencies.FindAsync(request.currencyId);
		string kerat21 = await (from c in context.Currencies
			where c.Code == "21K"
			select c.Rates).FirstOrDefaultAsync();
		double zakatIn21K = double.Parse(currency.Rates) / double.Parse(kerat21) * 0.025 * request.value;
		double userZakat = double.Parse(user.TotalZakatIn21k) - double.Parse(revenue.ZakatIn21k) + zakatIn21K;
		revenue.Value = request.value.ToString();
		revenue.CurrencyId = request.currencyId;
		revenue.ZakatIn21k = zakatIn21K.ToString();
		context.Revenues.Update(revenue);
		if (userZakat >= 2.125)
		{
			if (user.ZakatDeadLine == null)
			{
				DateTime gregorianDate = DateTime.Now;
				string hijriDate = ZakatUtil.UsingHijrahChronology(gregorianDate);
				user.ZakatDeadLine = hijriDate;
			}
			foreach (Revenue revenue2 in user.Revenues)
			{
				if (revenue2.ZakatRemaining == "0")
				{
					revenue2.ZakatRemaining = (double.Parse(revenue2.Value) * 0.025).ToString();
					context.Revenues.Update(revenue2);
				}
				revenue.ZakatRemaining = (double.Parse(revenue2.Value) * 0.025).ToString();
				context.Revenues.Update(revenue);
			}
		}
		else if (double.Parse(user.TotalZakatIn21k) > userZakat)
		{
			user.ZakatDeadLine = null;
			foreach (Revenue revenue3 in user.Revenues)
			{
				revenue3.ZakatRemaining = "0";
				context.Revenues.Update(revenue3);
			}
		}
		await context.SaveChangesAsync(cancellationToken);
		return await Task.FromResult(new Result<string>
		{
			Results = "Revenue Updated successfully."
		});
	}
}

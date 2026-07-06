using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;

namespace Application.Transactions.Commands;

internal sealed class CreateTransactionCommandHandler(IAppDbContext context) : IRequestHandler<CreateTransactionCommand, Result<string>>
{
	public async Task<Result<string>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
	{
		Transaction transaction = new Transaction();
		Revenue revenue = await context.Revenues.FindAsync(request.revenueId);
		Currency currency = await context.Currencies.FindAsync(request.currencyId);
		User user = await context.Users.FindAsync(request.userId);
		double paid = double.Parse(currency.Rates) * request.value / double.Parse(revenue.Currency.Rates);
		transaction.Currency = currency;
		transaction.Revenue = revenue;
		transaction.User = user;
		transaction.Value = request.value.ToString();
		await context.Transactions.AddAsync(transaction, cancellationToken);
		revenue.ZakatRemaining = (double.Parse(revenue.ZakatRemaining) - paid).ToString();
		if (revenue.ZakatPaid == null)
		{
			revenue.ZakatPaid = paid.ToString();
		}
		else
		{
			revenue.ZakatPaid = (double.Parse(revenue.ZakatPaid) + paid).ToString();
		}
		context.Revenues.Update(revenue);
		double kerat21K = double.Parse((from c in context.Currencies
			where c.Code == "21K"
			select c.Rates).FirstOrDefault());
		user.TotalPaidZakatIn21k = (double.Parse(user.TotalPaidZakatIn21k) + paid / kerat21K * double.Parse(revenue.Currency.Rates)).ToString();
		context.Users.Update(user);
		await context.SaveChangesAsync(cancellationToken);
		return await Task.FromResult(new Result<string>
		{
			Results = "Transaction added successfully."
		});
	}
}

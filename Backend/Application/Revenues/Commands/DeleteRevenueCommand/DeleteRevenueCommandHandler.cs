using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Revenues.Commands.DeleteRevenueCommand;

internal sealed class DeleteRevenueCommandHandler(IAppDbContext context) : IRequestHandler<DeleteRevenueCommand, Result<string>>
{
	public async Task<Result<string>> Handle(DeleteRevenueCommand request, CancellationToken cancellationToken)
	{
		Revenue revenue = await context.Revenues.Include((Revenue r) => r.User).FirstOrDefaultAsync((Revenue r) => r.RevenueId == int.Parse(request.id));
		User user = revenue.User;
		double totalZakat = double.Parse(user.TotalZakatIn21k);
		double remainingZakat = double.Parse(revenue.ZakatRemaining);
		user.TotalZakatIn21k = (totalZakat - remainingZakat).ToString();
		context.Revenues.Remove(revenue);
		await context.SaveChangesAsync(cancellationToken);
		return await Task.FromResult(new Result<string>
		{
			Results = "Revenue deleted successfully."
		});
	}
}

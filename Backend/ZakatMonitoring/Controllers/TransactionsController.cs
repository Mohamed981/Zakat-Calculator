using System.Threading;
using System.Threading.Tasks;
using API.Requests;
using Application.Common.Models;
using Application.Transactions.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequest request)
	{
		CreateTransactionCommand command = new CreateTransactionCommand(request.revenueId, request.currencyId, request.userId, request.value);
		return Ok(await mediator.Send((IRequest<Result<string>>)command, default(CancellationToken)));
	}
}

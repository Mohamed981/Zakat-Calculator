using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Requests;
using Application.Common.Models;
using Application.Currencies.Commands.UpdateCurrency;
using Application.Currencies.Queries;
using Application.Currencies.Queries.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CurrenciesController(IMediator mediator) : ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> GetCurrencies()
	{
		return Ok(await mediator.Send((IRequest<List<GetCurrenciesQueryResponse>>)new GetCurrenciesQuery(), default(CancellationToken)));
	}

	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateCurrency(int id, [FromBody] UpdateCurrencyRequest request)
	{
		UpdateCurrencyCommand command = new UpdateCurrencyCommand(id, request.Name, request.Value);
		return Ok(await mediator.Send((IRequest<Result<string>>)command, default(CancellationToken)));
	}
}

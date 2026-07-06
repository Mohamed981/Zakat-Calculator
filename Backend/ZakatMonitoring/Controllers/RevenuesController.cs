using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Requests;
using Application.Common.Models;
using Application.Revenues.Commands.CreateRevenueCommand;
using Application.Revenues.Commands.DeleteRevenueCommand;
using Application.Revenues.Commands.Models;
using Application.Revenues.Commands.UpdateRevenueCommand;
using Application.Revenues.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RevenuesController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> CreateRevenue([FromBody] CreateRevenueRequest request)
	{
		CreateRevenueCommand command = new CreateRevenueCommand(request.UserId, request.CurrencyId, request.Value);
		return Ok(await mediator.Send((IRequest<Result<string>>)command, default(CancellationToken)));
	}

	[HttpPut]
	public async Task<IActionResult> UpdateRevenue([FromBody] UpdateRevenueRequest request)
	{
		UpdateRevenueCommand command = new UpdateRevenueCommand(request.Id, request.UserId, request.CurrencyId, request.Value);
		return Ok(await mediator.Send((IRequest<Result<string>>)command, default(CancellationToken)));
	}

	[HttpGet]
	public async Task<IActionResult> GetRevenue()
	{
		GetRevenuesQuery query = new GetRevenuesQuery();
		return Ok(await mediator.Send((IRequest<List<GetRevenuesResponse>>)query, default(CancellationToken)));
	}

	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteRevenue(string id)
	{
		DeleteRevenueCommand deleteRevenue = new DeleteRevenueCommand(id);
		await mediator.Send((IRequest<Result<string>>)deleteRevenue, default(CancellationToken));
		return Ok(deleteRevenue);
	}
}

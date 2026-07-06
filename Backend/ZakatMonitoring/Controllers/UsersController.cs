using System.Threading;
using System.Threading.Tasks;
using API.Requests;
using Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IMediator mediator) : ControllerBase
{
	[HttpPost]
	public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
	{
		CreateUserCommand command = new CreateUserCommand(request.Name);
		return Ok(await mediator.Send((IRequest<string>)command, default(CancellationToken)));
	}
}

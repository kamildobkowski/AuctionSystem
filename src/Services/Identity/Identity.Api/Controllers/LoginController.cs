using Identity.Application.Features.RegisterPersonalUser;
using Microsoft.AspNetCore.Mvc;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Cqrs.Extensions;

namespace Identity.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
	[HttpPost("register/personal")]
	public async Task<IActionResult> Register([FromBody] RegisterPersonalUserCommand command,
		[FromServices] ICommandHandler<RegisterPersonalUserCommand, RegisterPersonalUserResponse> handler)
	{
		return (await handler.HandleAsync(command)).ToActionResult(201);
	}
}
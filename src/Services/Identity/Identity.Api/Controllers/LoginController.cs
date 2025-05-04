using Identity.Application.Features.RegisterPersonalUser;
using Microsoft.AspNetCore.Mvc;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Cqrs.Extensions;
using Shared.Base.Errors;

namespace Identity.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
	[HttpPost("register/personal")]
	[ProducesResponseType<RegisterPersonalUserResponse>(201)]
	[ProducesResponseType<ErrorResult>(400)]
	[ProducesResponseType(404)]
	public async Task<IActionResult> Register([FromBody] RegisterPersonalUserCommand command,
		[FromServices] ICommandHandler<RegisterPersonalUserCommand, RegisterPersonalUserResponse> handler)
	{
		return (await handler.HandleAsync(command)).ToActionResult(201);
	}
}
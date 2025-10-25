using Microsoft.AspNetCore.Mvc;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Cqrs.Extensions;
using Shared.Base.Errors;
using Identity.Application.Features.Login;
using Identity.Application.Features.RefreshToken;
using Identity.Application.Features.RegisterUser.RegisterCompanyUser;
using Identity.Application.Features.RegisterUser.RegisterPersonalUser;
using Microsoft.AspNetCore.Authorization;

namespace Identity.Api.Controllers;

[ApiController]
[AllowAnonymous]
[ProducesResponseType<ErrorResult>(400)]
public class LoginController : ControllerBase
{
	[HttpPost("register/personal")]
	[ProducesResponseType<RegisterPersonalUserResponse>(201)]
	[ProducesResponseType(404)]
	public async Task<IActionResult> Register([FromBody] RegisterPersonalUserCommand command,
		[FromServices] ICommandHandler<RegisterPersonalUserCommand, RegisterPersonalUserResponse> handler)
	{
		return (await handler.HandleAsync(command)).ToActionResult(201);
	}

	[HttpPost("register/company")]
	[ProducesResponseType<RegisterCompanyUserResponse>(201)]
	public async Task<IActionResult> RegisterCompany([FromBody] RegisterCompanyUserCommand command,
		[FromServices] ICommandHandler<RegisterCompanyUserCommand, RegisterCompanyUserResponse> handler)
	{
		return (await handler.HandleAsync(command)).ToActionResult(201);
	}

	[HttpPost("login")]
	[ProducesResponseType<LoginResponse>(200)]
	public async Task<IActionResult> Login([FromBody] LoginCommand command,
		[FromServices] ICommandHandler<LoginCommand, LoginResponse> handler)
	{
		var result = await handler.HandleAsync(command);
		return result.ToActionResult(200);
	}

	[HttpPost("refresh")]
	[ProducesResponseType<RefreshTokenResponse>(200)]
	public async Task<IActionResult> Refresh([FromBody] RefreshTokenCommand command,
		[FromServices] ICommandHandler<RefreshTokenCommand, RefreshTokenResponse> handler)
	{
		var result = await handler.HandleAsync(command);
		return result.ToActionResult(200);
	}
}
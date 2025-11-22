using Identity.Application.Features.UserData.GetUserData;
using Microsoft.AspNetCore.Mvc;
using Shared.Base.Cqrs.Extensions;
using Shared.Base.Cqrs.Queries;
using Shared.Base.Errors;

namespace Identity.Api.Controllers;

[ApiController]
[Route("userdata")]
public sealed class UserDataController : ControllerBase
{
	[HttpGet("{id:guid}")]
	[ProducesResponseType<GetUserDataQueryResponse>(200)]
	[ProducesResponseType<ErrorResult>(404)]
	public async Task<IActionResult> Get(
		[FromRoute] Guid id,
		[FromServices] IQueryHandler<GetUserDataQuery, GetUserDataQueryResponse> queryHandler)
		=> (await queryHandler.HandleAsync(new GetUserDataQuery(id))).ToActionResult();
}
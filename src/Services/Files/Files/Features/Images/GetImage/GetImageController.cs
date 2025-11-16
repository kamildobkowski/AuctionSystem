using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Base.Cqrs.Extensions;
using Shared.Base.Cqrs.Queries;

namespace Files.Features.Images.GetImage;

[ApiController]
[Tags("Images")]
[Authorize]
public sealed class GetImageController : ControllerBase
{
	[HttpGet("images/{id:guid}")]
	[ProducesResponseType(302)]
	[AllowAnonymous]
	public async Task<IActionResult> UploadImage(
		[FromRoute] Guid id,
		[FromServices] IQueryHandler<GetImageQuery, GetImageResponse> handler,
		CancellationToken cancellationToken)
	{
		var result = await handler.HandleAsync(new GetImageQuery(id), cancellationToken);
		if (result.IsSuccess)
			return Redirect(result.Result.FileUrl);
		return result.ToActionResult();
	}
}
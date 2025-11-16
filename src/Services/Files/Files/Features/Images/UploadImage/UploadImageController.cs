using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Cqrs.Extensions;

namespace Files.Features.Images.UploadImage;

[ApiController]
[Tags("Images")]
[Authorize]
public sealed class UploadImageController : ControllerBase
{
	[HttpPost("images/upload")]
	[ProducesResponseType<UploadImageResponse>(201)]
	public async Task<IActionResult> UploadImage(
		[FromForm] UploadImageCommand command,
		[FromServices] ICommandHandler<UploadImageCommand, UploadImageResponse> handler,
		CancellationToken cancellationToken)
	{
		return (await handler.HandleAsync(command, cancellationToken)).ToActionResult(201);
	}
}
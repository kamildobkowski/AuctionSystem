using Auctions.Application.Contracts.Picture;
using Microsoft.AspNetCore.Mvc;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Cqrs.Extensions;

namespace Auctions.Api.Controllers;

[ApiController]
[Route("picture")]
public class PictureController
{
	/*[HttpPost("add")]
	public async Task<IActionResult> AddPicture(
		[FromForm] List<IFormFile> files, 
		[FromForm] Guid auctionId, 
		[FromServices] ICommandHandler<AddPicturesCommand, AddPicturesCommandResponse> handler)
	{
		return (await handler.HandleAsync(new AddPicturesCommand(auctionId, files))).ToActionResult(201);
	}*/
}
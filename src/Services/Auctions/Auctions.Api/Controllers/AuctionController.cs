using Auctions.Application.BidAuction.Create;
using Microsoft.AspNetCore.Mvc;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Cqrs.Extensions;

namespace Auctions.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuctionController : ControllerBase
{
	[HttpPost("createBid")]
	[ProducesResponseType<CreateBidAuctionCommandResponse>(StatusCodes.Status201Created)]
	public async Task<IActionResult> CreateBidAuction(
		[FromBody] CreateBidAuctionCommand command,
		[FromServices] ICommandHandler<CreateBidAuctionCommand, CreateBidAuctionCommandResponse> handler,
		CancellationToken cancellationToken)
			=> (await handler.HandleAsync(command, cancellationToken)).ToActionResult(201);
}
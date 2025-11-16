using Auctions.Application.Contracts.BidAuction.InitializeCreate;
using Microsoft.AspNetCore.Mvc;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Cqrs.Extensions;

namespace Auctions.Api.Controllers;

[ApiController]
[Route("auctions/bid")]
public sealed class BidAuctionController : ControllerBase
{
	[HttpPost("create")]
	[ProducesResponseType<CreateBidAuctionCommandResponse>(StatusCodes.Status200OK)]
	public async Task<IActionResult> InitializeCreateBidAuction(
		[FromBody] CreateBidAuctionCommand command,
		[FromServices] ICommandHandler<CreateBidAuctionCommand, CreateBidAuctionCommandResponse> handler,
		CancellationToken cancellationToken)
			=> (await handler.HandleAsync(command, cancellationToken)).ToActionResult(201);
}
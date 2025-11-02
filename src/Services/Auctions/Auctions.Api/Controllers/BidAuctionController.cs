using Auctions.Application.Contracts.BidAuction.FinalizeCreate;
using Auctions.Application.Contracts.BidAuction.InitializeCreate;
using Microsoft.AspNetCore.Mvc;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Cqrs.Extensions;

namespace Auctions.Api.Controllers;

[ApiController]
[Route("auction/bid")]
public class BidAuctionController : ControllerBase
{
	[HttpPost("create/initialize")]
	[ProducesResponseType<InitializeCreateBidAuctionCommandResponse>(StatusCodes.Status200OK)]
	public async Task<IActionResult> InitializeCreateBidAuction(
		[FromBody] InitializeCreateBidAuctionCommand command,
		[FromServices] ICommandHandler<InitializeCreateBidAuctionCommand, InitializeCreateBidAuctionCommandResponse> handler,
		CancellationToken cancellationToken)
			=> (await handler.HandleAsync(command, cancellationToken)).ToActionResult();
	
	[HttpPost("create/finalize")]
	[ProducesResponseType(StatusCodes.Status201Created)]
	public async Task<IActionResult> FinalizeCreateBidAuction(
		[FromBody] FinalizeCreateBidAuctionCommand command,
		[FromServices] ICommandHandler<FinalizeCreateBidAuctionCommand, FinalizeCreateBidAuctionCommandResponse> handler,
		CancellationToken cancellationToken)
			=> (await handler.HandleAsync(command, cancellationToken)).ToActionResult(201);
}
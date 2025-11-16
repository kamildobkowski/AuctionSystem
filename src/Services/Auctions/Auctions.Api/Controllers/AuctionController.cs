using Auctions.Application.Contracts.AuctionList.GetUserShortList;
using Microsoft.AspNetCore.Mvc;
using Shared.Base.Cqrs.Extensions;
using Shared.Base.Cqrs.Queries;

namespace Auctions.Api.Controllers;

[ApiController]
[Route("auctions")]
public sealed class AuctionController : ControllerBase
{
	[HttpGet("my/shortList")]
	public async Task<IActionResult> GetUserAuctionsShortList(
		[FromQuery] GetUserAuctionShortListQuery query,
		[FromServices] IQueryHandler<GetUserAuctionShortListQuery, GetUserAuctionShortListQueryResponse> handler)
		=> (await handler.HandleAsync(query)).ToActionResult();
}
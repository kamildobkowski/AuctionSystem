using Auctions.Application.Contracts.AuctionDetails;
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

	[HttpGet("{slug}-{id:guid}")]
	public async Task<IActionResult> GetAuctionDetails(
		[FromRoute] string slug,
		[FromRoute] Guid id,
		[FromServices] IQueryHandler<GetAuctionDetailsQuery, GetAuctionDetailsResponse> handler)
	{
		var result = await handler.HandleAsync(new GetAuctionDetailsQuery(slug, id));
		if (!result.IsSuccess && result.ErrorResultOptional!.ErrorCode == "InvalidSlug")	
		{
			var redirectUrl = Url.Action(
				action: nameof(GetAuctionDetails),
				controller: "Auction",
				values: new { slug = result.ErrorResultOptional.ErrorDescription, id },
				protocol: Request.Scheme);

			return RedirectPermanent(redirectUrl!);
		}

		return result.ToActionResult();
	}
}
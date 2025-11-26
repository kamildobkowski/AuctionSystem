using Auctions.Domain.Common.Enums;
using Auctions.Domain.Entities;

namespace Auctions.Application.Common.Extensions;

public static class AuctionExtensions
{
	public static AuctionType GetAuctionType(this Auction auction)
	{
		return auction switch
		{
			Auctions.Domain.Entities.BidAuction => AuctionType.BidAuction,
			Auctions.Domain.Entities.BuyNowAuction => AuctionType.BuyNowAuction,
			_ => throw new ArgumentException("Auction type is not supported")
		};
	}
}
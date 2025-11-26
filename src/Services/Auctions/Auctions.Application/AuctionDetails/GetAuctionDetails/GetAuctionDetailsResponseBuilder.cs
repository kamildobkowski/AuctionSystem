using Auctions.Application.Common.Extensions;
using Auctions.Application.Common.Helpers;
using Auctions.Application.Contracts.AuctionDetails;
using Auctions.Domain.Entities;
using Shared.Base.Token;

namespace Auctions.Application.AuctionDetails.GetAuctionDetails;

public sealed class GetAuctionDetailsResponseBuilder(IFileHelper fileHelper, IUserContextProvider userContextProvider)
{
	public GetAuctionDetailsResponse Build(Auction auction, SellerDataModel sellerDataModel)
	{
		return new GetAuctionDetailsResponse(
			BuildDetails(auction),
            auction.DisplayPrice,
			sellerDataModel,
			auction.GetAuctionType(),
			IsUserSeller(auction.SellerId),
			auction.AuctionStats.Views,
			GetBidStatistics(auction)
		);
	}
	
	private AuctionDetailsModel BuildDetails(Auction auction)
	{
		return new AuctionDetailsModel(
			auction.Id,
			auction.Title,
			auction.Description,
			auction.SetEndDate,
			auction.EndedAt,
			auction.Status,
			auction.Pictures.OrderByDescending(x => x.IsPrimary).Select(x => fileHelper.GetFileUrl(x.Id)).ToList()
		);
	}
	
	private bool IsUserSeller(Guid sellerId)
	{
		try
		{
			var currentUserId = userContextProvider.GetUserId();
			return currentUserId == sellerId;
		}
		catch(Exception)
		{
			return false;
		}
	}

	private static BidStatisticsModel? GetBidStatistics(Auction auction)
	{
		if (auction is Auctions.Domain.Entities.BidAuction bidAuction)
		{
			return new BidStatisticsModel(
				0,
				0
			);
		}
		return null;
	}
}
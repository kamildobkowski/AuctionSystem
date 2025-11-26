using Auctions.Domain.Common.Enums;

namespace Auctions.Application.Contracts.AuctionDetails;

public sealed record GetAuctionDetailsResponse(
	AuctionDetailsModel Details,
	decimal Price,
	SellerDataModel Seller,
	AuctionType AuctionType,
	bool IsUserSeller,
	int NumberOfViews,
	BidStatisticsModel? BidStatisticsModel);
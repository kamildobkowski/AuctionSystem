using Auctions.Domain.Common.Enums;

namespace Auctions.Application.Contracts.AuctionDetails;

public sealed record GetAuctionDetailsResponse(
	AuctionDetailsModel DetailsModel,
	decimal Price,
	SellerDataModel Seller,
	AuctionType AuctionType,
	bool IsUserSeller,
	int NumberOfViews,
	BidStatisticsModel? BidStatisticsModel);
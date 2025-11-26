namespace Auctions.Application.Contracts.AuctionDetails;

public sealed record BidStatisticsModel(
	int IndividualBidders,
	int Bids);
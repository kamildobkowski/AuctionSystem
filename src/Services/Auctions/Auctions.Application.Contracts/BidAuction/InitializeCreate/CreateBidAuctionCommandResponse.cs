namespace Auctions.Application.Contracts.BidAuction.InitializeCreate;

public sealed record CreateBidAuctionCommandResponse(Guid BidAuctionId, string Slug);
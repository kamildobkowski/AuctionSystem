namespace Auctions.Application.Contracts.BidAuctions.InitializeCreate;

public sealed record CreateBidAuctionCommandResponse(Guid BidAuctionId, string Slug);
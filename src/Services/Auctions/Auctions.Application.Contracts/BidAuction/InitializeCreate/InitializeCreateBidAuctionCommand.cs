using Shared.Base.Cqrs.Commands;

namespace Auctions.Application.Contracts.BidAuction.InitializeCreate;

public sealed record InitializeCreateBidAuctionCommand(
	string Title, 
	string? Description, 
	decimal StartingPrice, 
	decimal? MinimalPrice,
	DateTime EndDate) : ICommand;
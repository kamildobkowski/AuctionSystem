using Shared.Base.Cqrs.Commands;

namespace Auctions.Application.Contracts.BidAuction.Create;

public sealed record CreateBidAuctionCommand(
	string Title, 
	string? Description, 
	decimal StartingPrice, 
	decimal? MinimalPrice,
	DateTime? StartDate,
	DateTime EndDate) : ICommand;
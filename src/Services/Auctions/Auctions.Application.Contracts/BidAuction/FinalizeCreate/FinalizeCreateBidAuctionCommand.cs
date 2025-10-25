using Shared.Base.Cqrs.Commands;

namespace Auctions.Application.Contracts.BidAuction.FinalizeCreate;

public sealed record FinalizeCreateBidAuctionCommand(Guid Id, DateTime? StartDate) : ICommand;
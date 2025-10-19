using Auctions.Domain.Entities;

namespace Auctions.Domain.Repositories;

public interface IBidAuctionRepository
{
	Task AddAsync(BidAuction auction, CancellationToken cancellationToken = default);
}
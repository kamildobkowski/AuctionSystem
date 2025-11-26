using Auctions.Domain.Entities;

namespace Auctions.Domain.Repositories;

public interface IAuctionRepository : IRepository<Auction, Guid>
{
	Task IncrementViewCountAsync(Guid auctionId);
}
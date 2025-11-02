using Auctions.Domain.Entities;
using Auctions.Domain.Repositories;
using Auctions.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Infrastructure.Repositories;

public class BidAuctionRepository(AuctionsDbContext dbContext) : GenericRepository<BidAuction, Guid>(dbContext), 
	IBidAuctionRepository
{
	protected override IQueryable<BidAuction> GetQueryable()
		=> dbContext.BidAuctions
			.Include(x => x.Pictures)
			.Include(x => x.AuctionStats)
			.AsNoTracking();
}
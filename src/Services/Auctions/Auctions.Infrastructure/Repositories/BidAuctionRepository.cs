using System.Linq.Expressions;
using Auctions.Domain.Entities;
using Auctions.Domain.Repositories;
using Auctions.Domain.UnitOfWork;
using Auctions.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Infrastructure.Repositories;

public class BidAuctionRepository(AuctionsDbContext dbContext) : IBidAuctionRepository
{
	public void Add(BidAuction auction, CancellationToken cancellationToken = default)
		=> dbContext.BidAuctions.Add(auction);

	public void Update(BidAuction entity, CancellationToken cancellationToken = default)
	{
		dbContext.Update(entity);
	}

	public void Delete(BidAuction entity, CancellationToken cancellationToken = default)
	{
		dbContext.BidAuctions.Remove(entity);
	}

	public async Task<BidAuction?> GetFirst(Expression<Func<BidAuction, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await GetBidAuctions.FirstOrDefaultAsync(predicate, cancellationToken);	
	}

	public async Task<BidAuction?> GetById(Guid id, CancellationToken cancellationToken = default)
	{
		return await GetBidAuctions.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
	}

	public async Task<List<BidAuction>> GetAll(Expression<Func<BidAuction, bool>> predicate, CancellationToken cancellationToken = default)
	{
		return await GetBidAuctions
			.Where(predicate)
			.ToListAsync(cancellationToken);
	}

	public IUnitOfWork UnitOfWork { get; init; } = new UnitOfWork(dbContext); 

	private IQueryable<BidAuction> GetBidAuctions
		=> dbContext.BidAuctions
			.Include(x => x.Pictures)
			.Include(x => x.AuctionStats)
			.AsNoTracking();
}
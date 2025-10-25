using Auctions.Domain.UnitOfWork;
using Auctions.Infrastructure.Database;

namespace Auctions.Infrastructure.Repositories;

public class UnitOfWork(AuctionsDbContext dbContext) : IUnitOfWork
{
	public Task SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
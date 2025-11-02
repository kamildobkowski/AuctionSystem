using Auctions.Domain.UnitOfWork;
using Auctions.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Infrastructure.Repositories;

public class UnitOfWork(DbContext dbContext) : IUnitOfWork
{
	public Task SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		return dbContext.SaveChangesAsync(cancellationToken);
	}
}
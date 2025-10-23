namespace Auctions.Domain.UnitOfWork;

public interface IUnitOfWork
{
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
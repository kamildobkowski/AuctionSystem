using System.Linq.Expressions;
using Auctions.Domain.UnitOfWork;

namespace Auctions.Domain.Repositories;

public interface IRepository<TEntity, in TId>
{
	void Add(TEntity entity, CancellationToken cancellationToken = default);
	
	void Update(TEntity entity, CancellationToken cancellationToken = default);
	
	void Delete(TEntity entity, CancellationToken cancellationToken = default);

	Task<TEntity?> GetFirst(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
	
	Task<TEntity?> GetById(TId id, CancellationToken cancellationToken = default);
	
	Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
	
	IUnitOfWork UnitOfWork { get; init; }
}
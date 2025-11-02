using System.Linq.Expressions;
using Auctions.Domain.Entities.Base;
using Auctions.Domain.Repositories;
using Auctions.Domain.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Auctions.Infrastructure.Repositories;

public abstract class GenericRepository<T, TId>(DbContext dbContext) 
	: IRepository<T, TId> where T : Aggregate<TId>
{
	public void Add(T entity, CancellationToken cancellationToken = default)
		=> dbContext.Set<T>().Add(entity);

	public void Update(T entity, CancellationToken cancellationToken = default)
		=> dbContext.Set<T>().Update(entity);

	public void Delete(T entity, CancellationToken cancellationToken = default)
		=> dbContext.Set<T>().Remove(entity);

	public Task<T?> GetFirst(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
		=> GetQueryable().FirstOrDefaultAsync(predicate, cancellationToken);

	public Task<T?> GetById(TId id, CancellationToken cancellationToken = default)
		=> GetQueryable().FirstOrDefaultAsync(x => x.Id!.Equals(id), cancellationToken);

	public Task<List<T>> GetAll(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
		=> GetQueryable().Where(predicate).ToListAsync(cancellationToken);

	public IUnitOfWork UnitOfWork { get; init; } = new UnitOfWork(dbContext);

	protected abstract IQueryable<T> GetQueryable();
}
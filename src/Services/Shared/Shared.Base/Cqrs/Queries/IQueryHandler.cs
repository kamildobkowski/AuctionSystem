namespace Shared.Base.Cqrs.Queries;

public interface IQueryHandler<TQuery, TResult>
	where TQuery : IQuery
{
	Task<IQueryResult<TResult>> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
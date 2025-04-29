using Shared.Base.Errors;

namespace Shared.Base.Cqrs.Queries;

public interface IQueryResult<T>
{
	public ErrorResult? ErrorResultOptional { get; init; }
	public T Result { get; init; }
	public bool IsSuccess { get; }
}
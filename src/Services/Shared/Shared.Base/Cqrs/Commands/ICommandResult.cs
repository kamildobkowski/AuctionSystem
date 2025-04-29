using Shared.Base.Errors;

namespace Shared.Base.Cqrs.Commands;

public interface ICommandResult<T>
{
	public ErrorResult? ErrorResultOptional { get; init; }
	public T Result { get; init; }
	public bool IsSuccess { get; }
}
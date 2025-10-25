using Shared.Base.Errors;

namespace Shared.Base.Cqrs.Commands;

public interface ICommandResult<out T>
{
	public ErrorResult? ErrorResult { get; }
	public T Result { get; }
	public bool IsSuccess { get; }
}
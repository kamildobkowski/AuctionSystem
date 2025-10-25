using Shared.Base.Errors;

namespace Shared.Base.Cqrs.Commands;

public static class CommandResult
{
	public static ICommandResult<T> Success<T>(T result)
		=> new CommandResult<T>(result);
	
	public static ICommandResult<T> Failure<T>(ErrorResult errorResult) 
		=> new CommandResult<T>(errorResult);
}

public readonly struct CommandResult<T> : ICommandResult<T>
{
	public ErrorResult? ErrorResult { get; }

	public T Result { get; } = default!;
	
	public bool IsSuccess => ErrorResult == null;

	public CommandResult(ErrorResult errorResult)
	{
		ErrorResult = errorResult;
	}

	public CommandResult(T result)
	{
		Result = result;
	}
}
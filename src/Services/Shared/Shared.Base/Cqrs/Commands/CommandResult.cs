using Shared.Base.Errors;

namespace Shared.Base.Cqrs.Commands;

public static class CommandResult
{
	public static ICommandResult<T> Success<T>(T result) 
		=> new CommandResult<T>
		{
			Result = result,
			ErrorResultOptional = null
		};
	
	public static ICommandResult<T> Failure<T>(ErrorResult errorResult) 
		=> new CommandResult<T>
		{
			Result = default!,
			ErrorResultOptional = errorResult
		};
}

public class CommandResult<T> : ICommandResult<T>
{
	public ErrorResult? ErrorResultOptional { get; init; }

	public T Result { get; init; } = default!;
	
	public bool IsSuccess => ErrorResultOptional == null;
}
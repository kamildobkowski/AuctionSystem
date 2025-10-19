using Shared.Base.Errors;

namespace Shared.Base.Result;

public static class Result
{
	public static Result<T> Ok<T>(T value) 
		=> new Result<T>
		{
			Value = value
		};

	public static Result<T> Failure<T>(ErrorResult error)
		=> new Result<T>
		{
			ErrorResultOptional = error
		};
	
	public static Result<T> Failure<T>(List<Error> errors)
		=> new Result<T>
		{
			ErrorResultOptional = ErrorResult.DomainError(errors)
		};
}

public class Result<T>
{
	public ErrorResult? ErrorResultOptional { get; init; }

	public T Value { get; init; } = default!;
	
	public bool IsSuccess => ErrorResultOptional == null;
}
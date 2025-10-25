using Shared.Base.Errors;

namespace Shared.Base.Result;

public readonly struct Result
{
	private Result(ErrorResult? errorResult = null)
	{
		ErrorResult = errorResult;
	}

	public ErrorResult? ErrorResult { get; }
	public bool IsSuccess => ErrorResult is null;


	public static Result Ok => new();

	public static Result Failure(ErrorResult error) => new(error);
	
	public static Result Failure(List<Error> errors) => new(ErrorResult.DomainError(errors));
}

public struct Result<T>
{
	public ErrorResult? ErrorResult { get; }

	public T Value { get; } = default!;
	
	public bool IsSuccess => ErrorResult is null;

	private Result(ErrorResult errorResult)
	{
		ErrorResult = errorResult;
	}

	private Result(T value)
	{
		Value = value;
	}

	public static Result<T> Ok(T value) => new(value);
	
	public static Result<T> Failure(ErrorResult error) => new(error);
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Cqrs.Queries;
using Shared.Base.Errors;
using Shared.Base.Result;

namespace Shared.Base.Cqrs.Extensions;

public static class ResultExtension
{
	public static IActionResult ToActionResult<T>(this ICommandResult<T> commandResult, int successStatusCode = StatusCodes.Status200OK)
	{
		if (commandResult.IsSuccess)
		{
			if (typeof(T) == typeof(NullResult) || successStatusCode == StatusCodes.Status204NoContent)
			{
				return new StatusCodeResult(successStatusCode);
			}

			return new ObjectResult(commandResult.Result)
			{
				StatusCode = successStatusCode
			};
		}

		if (commandResult.ErrorResult is null)
		{
			return new ObjectResult(ErrorResult.GenericError)
			{
				StatusCode = 500
			};
		}

		return new ObjectResult(commandResult.ErrorResult)
		{
			StatusCode = (int)commandResult.ErrorResult.StatusCode
		};
	}
	
	public static IActionResult ToActionResult<T>(this IQueryResult<T> commandResult, int successStatusCode = StatusCodes.Status200OK)
	{
		if (commandResult.IsSuccess)
		{
			if (typeof(T) == typeof(NullResult))
			{
				return new StatusCodeResult(successStatusCode);
			}
			return new ObjectResult(commandResult.Result)
			{
				StatusCode = successStatusCode
			};
		}

		if (commandResult.ErrorResultOptional is null)
		{
			return new ObjectResult(ErrorResult.GenericError)
			{
				StatusCode = 500
			};
		}

		return new ObjectResult(commandResult.ErrorResultOptional)
		{
			StatusCode = (int)commandResult.ErrorResultOptional.StatusCode
		};
	}
}
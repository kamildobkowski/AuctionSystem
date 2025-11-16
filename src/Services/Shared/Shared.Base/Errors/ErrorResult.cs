using System.Net;
using System.Text.Json.Serialization;
using FluentValidation.Results;

namespace Shared.Base.Errors;

public class ErrorResult
{
	public string? ErrorCode { get; set; }

	public string? ErrorDescription { get; set; }

	[JsonIgnore]
	public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.BadRequest;

	public IReadOnlyCollection<Error>? Errors { get; set; }

	public ErrorResult(){ }

	public ErrorResult(string errorCode, string? errorDescription = null, 
		HttpStatusCode statusCode = HttpStatusCode.BadRequest, IReadOnlyCollection<Error>? errors = null)
	{
		ErrorCode = errorCode;
		StatusCode = statusCode;
		ErrorDescription = errorDescription;
		Errors = errors;
	}

	public static readonly ErrorResult GenericError = new ErrorResult()
	{
		ErrorDescription = "Unknown error occured",
		ErrorCode = "UnknownError",
		StatusCode = HttpStatusCode.InternalServerError
	};

	public static readonly ErrorResult UnauthorizedError = new ErrorResult()
	{
		ErrorCode = nameof(UnauthorizedError),
		StatusCode = HttpStatusCode.Unauthorized
	};
	
	public static readonly ErrorResult NotFoundError = new ErrorResult()
	{
		ErrorCode = nameof(NotFoundError),
		ErrorDescription = "Resource not found",
		StatusCode = HttpStatusCode.NotFound
	};

	public static ErrorResult DomainError(List<Error>? errors = null) => new()
	{
		StatusCode = HttpStatusCode.BadRequest,
		ErrorDescription = "Domain error occured",
		ErrorCode = "DomainError",
		Errors = errors
	};

	public static ErrorResult ValidationError(ValidationResult validationResult) => new()
	{
		ErrorCode = "ValidationError",
		Errors =
			validationResult
				.Errors
				.Select(x => new Error
				{
					ErrorField = x.PropertyName,
					ErrorMessage = x.ErrorMessage,
					ErrorCode = x.ErrorCode
				})
				.ToList(),
		ErrorDescription = "Validation Error Occured"
	};
}
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

	public static ErrorResult GenericError = new ErrorResult()
	{
		ErrorDescription = "Unknown error occured",
		ErrorCode = "UnknownError",
		StatusCode = HttpStatusCode.InternalServerError
	};

	public static ErrorResult AuthorizationError = new ErrorResult()
	{
		ErrorDescription = "Authorization error occured",
		ErrorCode = "AuthorizationError",
		StatusCode = HttpStatusCode.BadRequest
	};

	public static ErrorResult DomainError(List<Error> errors) => new()
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
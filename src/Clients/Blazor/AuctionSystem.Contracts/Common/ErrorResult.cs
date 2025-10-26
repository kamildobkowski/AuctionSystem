using System.Net;
using System.Text.Json.Serialization;

namespace AuctionSystem.Contracts.Common;

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
}
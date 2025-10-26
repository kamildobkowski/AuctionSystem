using AuctionSystem.Contracts.Common;
using AuctionSystem.ExternalServices.Helpers;
using Refit;

namespace AuctionSystem.ExternalServices.Common;

internal abstract class ServiceBase
{
	protected async Task<Result<T>> Handle<T>(Func<Task<ApiResponse<T>>> func)
	{
		var response = await func();
		if (response.IsSuccessStatusCode)
			return Result<T>.Ok(response.Content!);
		var error = JsonHelper.Deserialize<ErrorResult>(response.Error.Content!);
		if (error is not null)
			return Result<T>.Failure(error);
		throw response.Error;
	} 
}
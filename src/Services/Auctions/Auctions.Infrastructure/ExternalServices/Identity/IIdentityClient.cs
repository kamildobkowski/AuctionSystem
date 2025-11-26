using Auctions.Infrastructure.ExternalServices.Identity.Models;
using Refit;

namespace Auctions.Infrastructure.ExternalServices.Identity;

internal interface IIdentityClient
{
	[Get("/userdata/{id}")]
	Task<ApiResponse<GetUserDataResponse>> GetUserDataAsync(Guid id);
}
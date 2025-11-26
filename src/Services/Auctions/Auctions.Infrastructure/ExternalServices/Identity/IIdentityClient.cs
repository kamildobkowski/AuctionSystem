using Auctions.Infrastructure.ExternalServices.Identity.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace Auctions.Infrastructure.ExternalServices.Identity;

internal interface IIdentityClient
{
	[HttpGet("userdata/{id:guid}")]
	Task<ApiResponse<GetUserDataResponse>> GetUserDataAsync(Guid id);
}
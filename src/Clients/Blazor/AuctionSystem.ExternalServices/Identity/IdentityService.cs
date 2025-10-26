using AuctionSystem.Contracts.Common;
using AuctionSystem.Contracts.Identity;
using AuctionSystem.Contracts.Identity.Models.Login;
using AuctionSystem.Contracts.Identity.Models.RegisterCompanyUser;
using AuctionSystem.Contracts.Identity.Models.RegisterPersonalUser;
using AuctionSystem.ExternalServices.Common;

namespace AuctionSystem.ExternalServices.Identity;

internal class IdentityService(IIdentityClient client) : ServiceBase, IIdentityService
{
	public Task<Result<RegisterPersonalUserResponse>> RegisterPersonalUser(RegisterPersonalUserRequest request)
		=> Handle(() => client.RegisterPersonalUser(request));

	public Task<Result<RegisterCompanyUserResponse>> RegisterCompanyUser(RegisterCompanyUserRequest request)
		=> Handle(() => client.RegisterCompanyUser(request));

	public Task<Result<LoginResponse>> Login(LoginRequest request)
		=> Handle(() => client.Login(request));
}
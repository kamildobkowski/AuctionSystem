using AuctionSystem.Contracts.Common;
using AuctionSystem.Contracts.Identity.Models.Login;
using AuctionSystem.Contracts.Identity.Models.RegisterCompanyUser;
using AuctionSystem.Contracts.Identity.Models.RegisterPersonalUser;

namespace AuctionSystem.Contracts.Identity;

public interface IIdentityService
{
	Task<Result<RegisterPersonalUserResponse>> RegisterPersonalUser(RegisterPersonalUserRequest request);
	Task<Result<RegisterCompanyUserResponse>> RegisterCompanyUser(RegisterCompanyUserRequest request);
	Task<Result<LoginResponse>> Login(LoginRequest request);
}
using AuctionSystem.Contracts.Identity.Models.Login;
using AuctionSystem.Contracts.Identity.Models.RegisterCompanyUser;
using AuctionSystem.Contracts.Identity.Models.RegisterPersonalUser;
using Refit;

namespace AuctionSystem.ExternalServices.Identity;

internal interface IIdentityClient
{
	[Post("/register/personal")]
	Task<ApiResponse<RegisterPersonalUserResponse>> RegisterPersonalUser(RegisterPersonalUserRequest request);
	
	[Post("/register/company")]
	Task<ApiResponse<RegisterCompanyUserResponse>> RegisterCompanyUser(RegisterCompanyUserRequest request);
	
	[Post("/login")]
	Task<ApiResponse<LoginResponse>> Login(LoginRequest request);
}
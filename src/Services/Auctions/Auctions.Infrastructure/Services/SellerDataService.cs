using System.Text.Json;
using Auctions.Application.AuctionDetails.Services;
using Auctions.Application.Contracts.AuctionDetails;
using Auctions.Infrastructure.ExternalServices.Identity;
using Auctions.Infrastructure.ExternalServices.Identity.Models;
using Shared.Base.Errors;
using Shared.Base.Result;

namespace Auctions.Infrastructure.Services;

internal sealed class SellerDataService(IIdentityClient identityClient) : ISellerDataService
{
	public async Task<Result<SellerDataModel>> GetSellerData(Guid sellerId)
	{
		var userData = await GetUserData(sellerId);
		if (!userData.IsSuccess)
			return Result<SellerDataModel>.Failure(userData.ErrorResult!);
		var sellerData = userData.Value.IsCompany ? MapCompanyUser(userData.Value) : MapPersonalUser(userData.Value);
		return Result<SellerDataModel>.Ok(sellerData);
	}

	private static SellerDataModel MapPersonalUser(GetUserDataResponse response)
	{
		return new SellerDataModel(
			false,
			response.PersonalUserData!.FirstName,
			response.PersonalUserData.PhoneNumber,
			null,
			null,
			null,
			null,
			null,
			null);
	}
	
	private static SellerDataModel MapCompanyUser(GetUserDataResponse response)
	{
		return new SellerDataModel(
			true,
			response.CompanyUserData!.Name,
			response.CompanyUserData.PhoneNumber,
			response.CompanyUserData.Nip,
			response.CompanyUserData.Address.Line1,
			response.CompanyUserData.Address.Line2,
			response.CompanyUserData.Address.City,
			response.CompanyUserData.Address.PostalCode,
			response.CompanyUserData.Address.CountryCode);
	}
	
	private async Task<Result<GetUserDataResponse>> GetUserData(Guid sellerId)
	{
		var response = await identityClient.GetUserDataAsync(sellerId);
		
		if (response.IsSuccessStatusCode && response.Content is not null)
			return Result<GetUserDataResponse>.Ok(response.Content);
		
		//error
		if (string.IsNullOrEmpty(response.Error?.Content))
			return Result<GetUserDataResponse>.Failure(ErrorResult.ExternalServiceError());
		var errorResult = JsonSerializer.Deserialize<ErrorResult>(response.Error.Content);
		return Result<GetUserDataResponse>.Failure(errorResult ?? ErrorResult.ExternalServiceError());
	}
}
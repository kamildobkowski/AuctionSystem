namespace Auctions.Infrastructure.ExternalServices.Identity.Models;

public sealed record GetUserDataResponse(
	string Email,
	bool IsCompany,
	PersonalUserData? PersonalUserData,
	CompanyUserData? CompanyUserData);
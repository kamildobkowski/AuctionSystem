namespace Identity.Application.Features.UserData.GetUserData;

public sealed record GetUserDataQueryResponse(
	string Email,
	bool IsCompany,
	PersonalUserData? PersonalUserData,
	CompanyUserData? CompanyUserData);
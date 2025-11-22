namespace Identity.Application.Features.UserData.GetUserData;

public sealed record PersonalUserData(
	string FirstName,
	string LastName,
	string? PhoneNumber);
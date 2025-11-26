namespace Auctions.Infrastructure.ExternalServices.Identity.Models;

public sealed record PersonalUserData(
	string FirstName,
	string LastName,
	string? PhoneNumber);
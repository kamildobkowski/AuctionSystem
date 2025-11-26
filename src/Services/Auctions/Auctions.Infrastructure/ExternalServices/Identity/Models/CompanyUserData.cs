namespace Auctions.Infrastructure.ExternalServices.Identity.Models;

public sealed record CompanyUserData(
	string Name,
	AddressModel Address,
	string Nip,
	string PhoneNumber);
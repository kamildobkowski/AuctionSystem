namespace Auctions.Application.Contracts.AuctionDetails;

public sealed record SellerDataModel(
	bool IsCompany,
	string Name,
	string? PhoneNumber,
	string? Nip,
	string? AddressLine1,
	string? AddressLine2,
	string? City,
	string? PostalCode,
	string? CountryCode);
namespace Auctions.Infrastructure.ExternalServices.Identity.Models;

public sealed record AddressModel(string Line1, string? Line2, string PostalCode, string City, string CountryCode);
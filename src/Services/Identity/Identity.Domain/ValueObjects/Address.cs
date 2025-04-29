using Maddalena;

namespace Identity.Domain.ValueObjects;

public sealed class Address
{
	public string Line1 { get; set; } = default!;
	public string? Line2 { get; set; }
	public string PostalCode { get; set; } = default!;
	public string City { get; set; } = default!;
	public CountryCode Country { get; set; } = CountryCode.PL;
}
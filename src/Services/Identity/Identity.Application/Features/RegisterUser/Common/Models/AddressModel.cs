using Identity.Domain.ValueObjects;
using Maddalena;

namespace Identity.Application.Features.RegisterUser.Common.Models;

public record AddressModel(string Line1, string? Line2, string PostalCode, string City, string CountryCode)
{
	public Address ToEntity()
	{
		return new Address
		{
			Line1 = Line1,
			Line2 = Line2,
			PostalCode = PostalCode,
			City = City,
			Country = Enum.Parse<CountryCode>(CountryCode)
		};
	}

	public AddressModel(Address address) : this(address.Line1, address.Line2, address.PostalCode, address.City, address.Country.ToString())
	{
		
	}
}
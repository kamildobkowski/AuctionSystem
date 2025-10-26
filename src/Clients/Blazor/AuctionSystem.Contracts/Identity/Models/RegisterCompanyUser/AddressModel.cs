using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.Contracts.Identity.Models.RegisterCompanyUser;

public sealed class AddressModel
{
	[Required(ErrorMessage = "Address line 1 is required.")] 
	[MaxLength(200, ErrorMessage = "Address line 1 is too long.")]
	public string Line1 { get; set; } = null!;
	
	[MaxLength(200, ErrorMessage = "Address line 2 is too long.")]
	public string? Line2 { get; set; }

	[Required(ErrorMessage = "Postal code is required.")] 
	[MaxLength(10, ErrorMessage = "Postal code is too long.")]
	public string PostalCode { get; set; } = null!;

	[Required(ErrorMessage = "City is required.")]
	[MaxLength(100, ErrorMessage = "City is too long.")]
	public string City { get; set; } = null!;
	
	[Required(ErrorMessage = "Country code is required.")]
	public string CountryCode { get; set; } = null!;
}
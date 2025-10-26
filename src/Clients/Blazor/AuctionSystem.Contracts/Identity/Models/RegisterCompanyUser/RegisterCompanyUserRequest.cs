using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.Contracts.Identity.Models.RegisterCompanyUser;

public sealed class RegisterCompanyUserRequest
{
	[Required(ErrorMessage = "Name is required.")]
	[StringLength(100, ErrorMessage = "Name is too long.")]
	public string Name { get; set; } = null!;
	
	[Required(ErrorMessage = "Email is required.")]
	[EmailAddress(ErrorMessage = "Invalid email address.")]
	public string Email { get; set; } = null!;

	[Required(ErrorMessage = "Password is required.")]
	[RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$",
		ErrorMessage = "Password must have 8+ chars, uppercase, number and special char.")]
	public string Password { get; set; } = null!;

	[Required(ErrorMessage = "Repeat password.")]
	[Compare(nameof(Password), ErrorMessage = "Passwords must match.")]
	public string RepeatPassword { get; set; } = null!;

	[Required(ErrorMessage = "Tax ID is required.")]
	[StringLength(30, ErrorMessage = "Tax ID is too long.")]
	public string TaxId { get; set; } = null!;

	[Required(ErrorMessage = "Prefix is required.")]
	[RegularExpression(@"^\+\d{1,4}$", ErrorMessage = "Prefix like +48.")]
	public string PhonePrefix { get; set; } = null!;

	[Required(ErrorMessage = "Phone number is required.")]
	[RegularExpression(@"^\d{6,15}$", ErrorMessage = "6–15 digits.")]
	public string PhoneNumber { get; set; } = null!;

	public AddressModel Address { get; set; } = new();
}
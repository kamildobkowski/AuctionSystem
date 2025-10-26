using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.Contracts.Identity.Models.RegisterPersonalUser;

public sealed class RegisterPersonalUserRequest
{
	[Required(ErrorMessage = "First name is required.")]
	[StringLength(100, ErrorMessage = "First name is too long.")]
	public string FirstName { get; set; } = string.Empty;

	[Required(ErrorMessage = "Last name is required.")]
	[StringLength(100, ErrorMessage = "Last name is too long.")]
	public string LastName { get; set; } = string.Empty;

	[Required(ErrorMessage = "Email is required.")]
	[EmailAddress(ErrorMessage = "Invalid email address.")]
	public string Email { get; set; } = string.Empty;

	[Required(ErrorMessage = "Password is required.")]
	[RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$",
		ErrorMessage = "Password must have 8+ chars, uppercase, number and special char.")]
	public string Password { get; set; } = string.Empty;

	[Required(ErrorMessage = "Repeat password.")]
	[Compare(nameof(Password), ErrorMessage = "Passwords must match.")]
	public string RepeatPassword { get; set; } = string.Empty;

	[Required(ErrorMessage = "Prefix is required.")]
	[RegularExpression(@"^\+\d{1,4}$", ErrorMessage = "Prefix like +48.")]
	public string PhonePrefix { get; set; } = string.Empty;

	[Required(ErrorMessage = "Phone number is required.")]
	[RegularExpression(@"^\d{6,15}$", ErrorMessage = "6–15 digits.")]
	public string PhoneNumber { get; set; } = string.Empty;
}
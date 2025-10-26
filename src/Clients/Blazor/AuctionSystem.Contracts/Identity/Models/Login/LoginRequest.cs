using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.Contracts.Identity.Models.Login;

public sealed class LoginRequest
{
	[Required(ErrorMessage = "Email is required.")]
	[EmailAddress(ErrorMessage = "Invalid email address format.")]
	public string Email { get; set; } = null!;

	[Required(ErrorMessage = "Password is required.")]
	public string Password { get; set; } = null!;
}
namespace AuctionSystem.Contracts.Identity.Models.Login;

public sealed class LoginResponse
{
	public string AccessToken { get; set; } = null!;

	public string RefreshToken { get; set; } = null!;
}
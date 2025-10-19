namespace Identity.Domain.Entities;

public sealed class RefreshToken(Guid userId, string tokenHash)
{
	public Guid UserId { get; set; } = userId;
	public string TokenHash { get; set; } = tokenHash;
}
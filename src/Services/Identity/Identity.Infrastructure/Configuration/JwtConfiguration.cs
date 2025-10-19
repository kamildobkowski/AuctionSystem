namespace Identity.Infrastructure.Configuration;

public class JwtConfiguration
{
	public int AccessTokenLifetimeMinutes { get; set; } = 15;
	public TimeSpan AccessTokenLifetime => TimeSpan.FromMinutes(AccessTokenLifetimeMinutes);
	public int RefreshTokenLifetimeMinutes { get; set; } = 300;
	public TimeSpan RefreshTokenLifetime => TimeSpan.FromMinutes(RefreshTokenLifetimeMinutes);
	public string Secret { get; set; } = string.Empty;
	public string Issuer { get; set; } = string.Empty;
	public string Audience { get; set; } = string.Empty;
}
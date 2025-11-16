namespace Auctions.Infrastructure.Configuration;

public sealed class FileConfiguration
{
	public string GatewayUrl { get; set; } = null!;
	public string Endpoint { get; set; } = null!;
	
	public string FileUrl(string id) => string.Format(string.Concat(GatewayUrl, Endpoint), id);
}
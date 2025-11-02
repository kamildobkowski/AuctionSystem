namespace Auctions.Infrastructure.Configuration;

public sealed class MinioConfiguration
{
	public const string SectionName = "Minio";

	public string EndpointName { get; set; } = null!;
	
	public string PublicEndpointName { get; set; } = null!;
}
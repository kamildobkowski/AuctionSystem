using System.Text.Json;
using AuctionSystem.Contracts.Identity;
using AuctionSystem.ExternalServices.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace AuctionSystem.ExternalServices;

public static class DependencyInjectionExtension
{
	public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration configuration)
	{
		var refitSettings = new RefitSettings
		{
			ContentSerializer = new SystemTextJsonContentSerializer(
				new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
		};

		services
			.AddRefitClient<IIdentityClient>(refitSettings)
			.ConfigureHttpClient(c =>
			{
				c.BaseAddress = new Uri(configuration["ExternalServices:Identity:BaseUrl"]!);
			});

		services.AddScoped<IIdentityService, IdentityService>();
		
		return services;
	}	
}
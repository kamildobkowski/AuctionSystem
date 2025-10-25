using Auctions.Domain.Repositories;
using Auctions.Infrastructure.Database;
using Auctions.Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Events.Events.Auctions;

namespace Auctions.Infrastructure;

public static class DependencyInjectionExtension
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDatabase(configuration);

		services.SetupMassTransit();
		
		return services;
	}

	private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddDbContext<AuctionsDbContext>(x =>
		{
			x.UseNpgsql(configuration.GetConnectionString("postgres"));
			x.ConfigureWarnings(x => x.Ignore(RelationalEventId.PendingModelChangesWarning));
		});

		services.AddScoped<IBidAuctionRepository, BidAuctionRepository>();
		
		return services;
	}

	private static IServiceCollection SetupMassTransit(this IServiceCollection services)
	{
		services.AddMassTransit(x =>
		{
			x.UsingInMemory();
			x.AddEntityFrameworkOutbox<AuctionsDbContext>(x =>
			{
				x.QueryDelay = TimeSpan.FromSeconds(1);
				x.DuplicateDetectionWindow = TimeSpan.FromMinutes(10);
				x.UsePostgres();
			});
			x.AddRider(rider =>
			{
				rider.AddProducer<BidAuctionCreatedEvent>(BidAuctionCreatedEvent.Topic);
				rider.UsingKafka((context, configurator) =>
				{
					configurator.Host("vpn.kamildobkowski.pl:9092");
				});
			});
		});
		
		return services;
	}
}
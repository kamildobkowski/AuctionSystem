using Auctions.Application.AuctionDetails.Services;
using Auctions.Application.AuctionList.Services;
using Auctions.Application.Common.Helpers;
using Auctions.Application.Common.Jobs;
using Auctions.Domain.Repositories;
using Auctions.Infrastructure.Configuration;
using Auctions.Infrastructure.Database;
using Auctions.Infrastructure.ExternalServices.Identity;
using Auctions.Infrastructure.Helpers;
using Auctions.Infrastructure.Repositories;
using Auctions.Infrastructure.Schedulers;
using Auctions.Infrastructure.Services;
using Hangfire;
using Hangfire.Redis.StackExchange;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Shared.Base.Http;
using Shared.Events.Events.Auctions;
using Shared.Events.Events.Files;

namespace Auctions.Infrastructure;

public static class DependencyInjectionExtension
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
	{
		var jwtConfig = new FileConfiguration();
		configuration.GetSection("File").Bind(jwtConfig);
		services.AddSingleton(jwtConfig);
		services.AddHangfire(config => config.UseRedisStorage(configuration.GetConnectionString("redis")));
		
		services.AddSingleton<Auctions.Application.Common.Helpers.IFileHelper, FileHelper>();
		services.AddScoped<IFireAndForgetScheduler, HangfireScheduler>();
		
		services.AddDatabase(configuration);

		services.SetupMassTransit(configuration);
		
		services.SetupExternalServices(configuration);
		
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
		services.AddScoped<IAuctionRepository, AuctionRepository>();
		services.AddScoped<IAuctionListReadRepository, AuctionRepository>();
		
		return services;
	}

	private static IServiceCollection SetupMassTransit(this IServiceCollection services, IConfiguration configuration)
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
				rider.AddProducer<SetImageToUsedCommand>(SetImageToUsedCommand.Topic);
				rider.UsingKafka((context, configurator) =>
				{
					configurator.Host(configuration["Kafka:BootstrapServers"]);
				});
			});
		});
		
		return services;
	}

	private static IServiceCollection SetupExternalServices(this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddRefitClient<IIdentityClient>()
			.ConfigureHttpClient(c =>
			{
				c.BaseAddress = new Uri(configuration["ExternalServices:Identity:BaseUrl"]!);
			})
			.AddHttpMessageHandler<BearerTokenDelegatingHandler>();
		services.AddSingleton<ISellerDataService, SellerDataService>();
		
		return services;
	}
}
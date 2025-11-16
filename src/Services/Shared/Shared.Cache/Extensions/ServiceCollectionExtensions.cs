using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Cache.Abstractions;
using Shared.Cache.Implementation;
using StackExchange.Redis;
using StackExchange.Redis.KeyspaceIsolation;

namespace Shared.Cache.Extensions;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddCache(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddSingleton<IDatabase>(x =>
		{
			var connectionString = configuration.GetSection("ConnectionStrings")["redis"];
			var appName = configuration.GetValue<string>("AppName");
			if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(appName))
				throw new InvalidOperationException("redis configuration is missing");
			var connection = ConnectionMultiplexer.Connect(connectionString);
			return connection.GetDatabase().WithKeyPrefix(string.Concat(appName, '_'));
		});
		
		services.AddSingleton<ICache, RedisCache>();
		
		return services;
	}
}
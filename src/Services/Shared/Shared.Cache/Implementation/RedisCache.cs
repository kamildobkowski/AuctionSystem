using System.Text.Json;
using Shared.Cache.Abstractions;
using StackExchange.Redis;

namespace Shared.Cache.Implementation;

internal sealed class RedisCache(IDatabase redis) : ICache
{
	public async Task<T?> Get<T>(string key)
	{
		var rawValue = await redis.StringGetAsync(key);
		if (string.IsNullOrEmpty(rawValue))
			return default;
		var value = JsonSerializer.Deserialize<T>(rawValue!);
		return value;
	}

	public async Task<T?> GetFromHash<T>(string hash, string key)
	{
		var rawValue = await redis.HashGetAsync(hash, key);
		if (string.IsNullOrEmpty(rawValue))
			return default;
		var value = JsonSerializer.Deserialize<T>(rawValue!);
		return value;
	}

	public Task Set<T>(string key, T value, TimeSpan? expiration = null)
	{
		var rawValue = JsonSerializer.Serialize(value);
		return redis.StringSetAsync(key, rawValue, expiration);
	}
	
	public Task SetHash<T>(string hash, string key, T value)
	{
		var rawValue = JsonSerializer.Serialize(value);
		return redis.HashSetAsync(hash, key, rawValue);
	}
}
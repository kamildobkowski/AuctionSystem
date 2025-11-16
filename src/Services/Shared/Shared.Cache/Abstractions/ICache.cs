namespace Shared.Cache.Abstractions;

public interface ICache
{
	Task<T?> Get<T>(string key);

	Task Set<T>(string key, T value, TimeSpan? expiration = null);
	Task<T?> GetFromHash<T>(string hash, string key);
	Task SetHash<T>(string hash, string key, T value);
}
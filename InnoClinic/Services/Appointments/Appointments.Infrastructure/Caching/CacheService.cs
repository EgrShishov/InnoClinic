using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Concurrent;

public class CacheService : ICacheService
{
    private IDistributedCache _distributedCache;
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new(); 
    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }
    public async Task<T?> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default) where T : class
    {
        string? cachedValue = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);

        if (cachedValue is null) 
        {
            return null;
        }

        T? value = JsonConvert.DeserializeObject<T>(cachedValue);
        
        return value;
    }

    public async Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(cacheKey, cancellationToken);
        
        CacheKeys.TryRemove(cacheKey, out bool _);
    }

    public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
    {
        foreach (string key in CacheKeys.Keys)
        {
            if (key.StartsWith(prefixKey))
            {
                await RemoveAsync(key, cancellationToken);
            }
        }
    }

    public async Task SetAsync<T>(string cacheKey, T value, CancellationToken cancellationToken = default) where T : class
    {
        string cachedValue = JsonConvert.SerializeObject(value);

        await _distributedCache.SetStringAsync(cacheKey, cachedValue, cancellationToken);

        CacheKeys.TryAdd(cacheKey, false);
    }

    public async Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class
    {
        T? cachedValue = await GetAsync<T>(cacheKey, cancellationToken);

        if (cachedValue is not null)
        {
            return cachedValue;
        }

        cachedValue = await factory();

        await SetAsync(cacheKey, cachedValue, cancellationToken);

        return cachedValue;
    }
}

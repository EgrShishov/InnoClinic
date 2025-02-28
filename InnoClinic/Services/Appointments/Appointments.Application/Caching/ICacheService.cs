public interface ICacheService
{
    public Task<T?> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default)
        where T : class;    
    public Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> factory, CancellationToken cancellationToken = default)
        where T : class;
    public Task SetAsync<T>(string cacheKey, T value, CancellationToken cancellationToken = default)
        where T : class;
    public Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default);
    public Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default);
}
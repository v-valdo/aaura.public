using Microsoft.Extensions.Caching.Memory;

namespace aaura.api.Services;

public class CacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<CacheService> _logger;
    public CacheService(IMemoryCache memoryCache, ILogger<CacheService> logger)
    {
        _cache = memoryCache;
        _logger = logger;
    }

    public async Task<T> GetOrSetAsync<T>(string cacheKey, Func<Task<T>> getDataFunc, TimeSpan cacheDuration)
    {
        if (!_cache.TryGetValue(cacheKey, out T cacheValue))
        {
            _logger.LogInformation($"======= No cache stored in: {cacheKey}. Generating new data ======");

            cacheValue = await getDataFunc();
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheDuration
            };
            _cache.Set(cacheKey, cacheValue, cacheEntryOptions);
            _logger.LogInformation($"Cache CREATED for key: {cacheKey}");
        }
        else
        {
            _logger.LogInformation($"Cache READING from key: {cacheKey}");
        }
        return cacheValue;
    }

    public async Task ClearAsync(string key)
    {
        _cache.Remove(key);
        _logger.LogInformation($"{key} cleared");
        await Task.CompletedTask;
    }
}
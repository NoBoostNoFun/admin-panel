using System;
using System.Text.Json;
using System.Threading.Tasks;
using AdminPanel.Exceptions;
using Microsoft.Extensions.Caching.Distributed;

namespace AdminPanel.Cache;

public static class DistributedCacheExtensions
{
    public static async Task<T?> GetAsync<T>(this IDistributedCache cache, string key) where T : class
    {
        try
        {
            var cachedResult = await cache.GetStringAsync(key);
            return string.IsNullOrWhiteSpace(cachedResult)
                ? null
                : JsonSerializer.Deserialize<T>(cachedResult);
        }
        catch (Exception e)
        {
            throw new CacheException("Couldn't get data from cache", e);
        }
    }

    public static async Task SetAsync(this IDistributedCache cache, string key, object data, int expirationTimeMin)
    {
        try
        {
            var value = JsonSerializer.Serialize(data);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationTimeMin),
            };
            await cache.SetStringAsync(key, value, options);
        }
        catch (Exception e)
        {
            throw new CacheException("Couldn't set data to cache", e);
        }
    }
}

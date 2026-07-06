using System.Text.Json;
using Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Caching;

internal sealed class RedisCacheService(IDistributedCache cache) : ICacheService
{
    private static readonly TimeSpan DefaultTtl = TimeSpan.FromHours(1);

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        string? cached = await cache.GetStringAsync(key, cancellationToken);
        return cached is null ? default : JsonSerializer.Deserialize<T>(cached);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? ttl, CancellationToken cancellationToken)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl ?? DefaultTtl
        };
        await cache.SetStringAsync(key, JsonSerializer.Serialize(value), options, cancellationToken);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        return cache.RemoveAsync(key, cancellationToken);
    }
}

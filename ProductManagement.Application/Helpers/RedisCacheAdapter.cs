using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Options;
using ProductManagement.Application.Helpers.Policies;

namespace ProductManagement.Application.Helpers
{
    public class RedisCacheAdapter : IRedisDistributedCache
    {
        private readonly RedisCache _redisCache;

        public RedisCacheAdapter(IOptions<RedisCacheOptions> options)
        {
            _redisCache = new RedisCache(options);
        }

        public byte[]? Get(string key) => _redisCache.Get(key);

        public Task<byte[]?> GetAsync(string key, CancellationToken token = default) => 
            _redisCache.GetAsync(key, token);

        public void Refresh(string key) => _redisCache.Refresh(key);

        public Task RefreshAsync(string key, CancellationToken token = default) => 
            _redisCache.RefreshAsync(key, token);

        public void Remove(string key) => _redisCache.Remove(key);

        public Task RemoveAsync(string key, CancellationToken token = default) => 
            _redisCache.RemoveAsync(key, token);

        public void Set(string key, byte[] value, DistributedCacheEntryOptions options) => 
            _redisCache.Set(key, value, options);

        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default) => 
            _redisCache.SetAsync(key, value, options, token);
    }
}

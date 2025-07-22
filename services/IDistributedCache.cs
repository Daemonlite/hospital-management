using Microsoft.Extensions.Caching.Distributed;

namespace Health.services
{
    public class RedisCacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task CacheSetAsync(string key, string value, int expireMinutes = 10)
        {
            await _cache.SetStringAsync(key, value, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expireMinutes)
            });
        }

        public async Task<string> CacheGetAsync(string key)
        {
            return await _cache.GetStringAsync(key)??string.Empty;
        }
    }

}
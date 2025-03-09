using GymManagement.Core.BaseObjects;
using GymManagement.Core.Configurations;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace GymManagement.Core.CacheManager
{
    public abstract class CacheManager<T>(IMemoryCache _memoryCache, IOptions<CachingOptions> cachingOptions) where T : BaseDao
    {
        private readonly CachingOptions _cachingOptions = cachingOptions.Value;

        protected abstract string CacheKey { get; }

        public T? GetCached(int id)
        {
            return _memoryCache.Get<T>(GetCacheKey(id));
        }

        public void Cache(T objectToCache)
        {
            _memoryCache.Set(GetCacheKey(objectToCache.Id), objectToCache, TimeSpan.FromSeconds(_cachingOptions.CacheDurationInSeconds));
        }

        public void Invalidate(int id)
        {
            _memoryCache.Remove(GetCacheKey(id));
        }

        public void Invalidate(int id, string cacheKey)
        {
            _memoryCache.Remove(string.Format(cacheKey, id));
        }

        private string GetCacheKey(int id)
        {
            return string.Format(CacheKey, id);
        }
    }
}

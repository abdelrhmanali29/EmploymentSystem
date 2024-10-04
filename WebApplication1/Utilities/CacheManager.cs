using Microsoft.Extensions.Caching.Memory;

namespace EmploymentSystem.Utilities
{
    public static class CacheManager
    {
        private static readonly MemoryCache Cache = new MemoryCache(new MemoryCacheOptions());

        public static void Set(string key, object value, TimeSpan duration)
        {
            Cache.Set(key, value, duration);
        }

        public static T? Get<T>(string key)
        {
            return Cache.TryGetValue(key, out T value) ? value : default;
        }

        public static void Remove(string key)
        {
            Cache.Remove(key);
        }
    }

}

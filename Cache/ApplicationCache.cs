using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Caching.Memory;

namespace RoltaTimeSheet.Cache
{
    public sealed class ApplicationCache
    {
        private static readonly ApplicationCache instance = new ApplicationCache();
        private readonly MemoryCache cache;

        private ApplicationCache()
        {
            cache = new MemoryCache(new MemoryCacheOptions());
        }

        public static ApplicationCache Instance
        {
            get { return instance; }
        }

        public void Add(string key, object value)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
            };

            cache.Set(key, value, cacheEntryOptions);
        }

        public object Get(string key)
        {
            return cache.TryGetValue(key, out var value) ? value : null;
        }

        public void Remove(string key)
        {
            cache.Remove(key);
        }
    }


}
using System;
using CacheStrategy.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace CacheStrategy.Stores.Caching
{

    public class CarCachingStore<T> : ICarStore
        where T : ICarStore
    {
        private readonly IMemoryCache _memoryCache;
        private readonly T _inner;
        private readonly ILogger<CarCachingStore<T>> _logger;

        public CarCachingStore(IMemoryCache memoryCache, T inner, ILogger<CarCachingStore<T>> logger)
        {
            _memoryCache = memoryCache;
            _inner = inner;
            _logger = logger;
        }

        public IEnumerable<Car> List()
        {
            var key = "Cars";
            var item = _memoryCache.Get<IEnumerable<Car>>(key);

            if (item == null)
            {
                item = _inner.List();
                if (item != null)
                {
                    _memoryCache.Set(key, item, TimeSpan.FromMinutes(1));
                }
            }

            return item;
        }

        public Car Get(int id)
        {
            var key = GetKey(id.ToString());
            var item = _memoryCache.Get<Car>(key);

            if (item == null)
            {
                _logger.LogTrace("Cache miss for {cacheKey}", key);
                item = _inner.Get(id);
                if (item != null)
                {
                    _logger.LogTrace("Setting item in cache for {cacheKey}", key);
                    _memoryCache.Set(key, item, TimeSpan.FromMinutes(1));
                }
            }
            else
            {
                _logger.LogTrace("Cache hit for {cacheKey}", key);
            }

            return item;
        }


        private static string GetKey(string key)
        {
            return $"{typeof(T).FullName}:{key}";
        }
    }
}

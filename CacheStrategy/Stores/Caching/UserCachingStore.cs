using CacheStrategy.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace CacheStrategy.Stores.Caching
{
    public class UserCachingStore<T> : IUserStore
        where T : IUserStore
    {

        private readonly IMemoryCache _memoryCache;
        private readonly T _inner;
        private readonly ILogger<UserCachingStore<T>> _logger;

        public UserCachingStore(IMemoryCache memoryCache, T inner, ILogger<UserCachingStore<T>> logger)
        {
            _memoryCache = memoryCache;
            _inner = inner;
            _logger = logger;
        }
        public IEnumerable<User> List()
        {
            var key = "Users";
            var item = _memoryCache.Get<IEnumerable<User>>(key);

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

        public User Get(int userid)
        {
            var key = GetKey(userid.ToString());
            var item = _memoryCache.Get<User>(key);

            if (item == null)
            {
                _logger.LogTrace("Cache miss for {cacheKey}", key);
                item = _inner.Get(userid);
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

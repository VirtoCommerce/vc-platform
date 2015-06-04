using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks.Caching
{
    public class InMemoryCachingProvider : ICacheProvider
    {
        private Dictionary<string, CachedObject> _cache = new Dictionary<string, CachedObject>();
        private ICacheKeyGenerator _keyGenerator = new ExpressionCacheKeyGenerator();

        public int MissCount { get; set; }
        public int TotalCount { get; set; }

        #region ICacheProvider Members

        public ICacheKeyGenerator KeyGenerator
        {   
            get { return _keyGenerator; }
        }

        public TimeSpan GetExpirationTimeout(CacheKey key)
        {
            return TimeSpan.FromDays(1);
        }

        public void Put(CacheKey key, CachedObject cachedObj, TimeSpan expirationTimeout)
        {
            _cache[key.Key] = cachedObj;
        }

        public CachedObject Get(CacheKey key)
        {
            CachedObject retVal;
            TotalCount++;
            if (!_cache.TryGetValue(key.Key, out retVal))
            {
                MissCount++;
            }
            return retVal;
        }

        #endregion
    }
}

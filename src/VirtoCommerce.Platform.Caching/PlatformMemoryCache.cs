using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Caching
{
    public class PlatformMemoryCache : IPlatformMemoryCache
    {
        private readonly IMemoryCache _memoryCache;
        private bool _disposed;
        private readonly ILogger _log;

        public PlatformMemoryCache(IMemoryCache memoryCache, IOptions<CachingOptions> options, ILogger<PlatformMemoryCache> log)
        {
            _memoryCache = memoryCache;
            var cachingOptions = options.Value;
            CacheEnabled = cachingOptions.CacheEnabled;
            AbsoluteExpiration = cachingOptions.CacheAbsoluteExpiration;
            SlidingExpiration = cachingOptions.CacheSlidingExpiration;
            _log = log;
        }
        
        public virtual ICacheEntry CreateEntry(object key)
        {
            var result = _memoryCache.CreateEntry(key);
            if (result != null)
            {
                result.RegisterPostEvictionCallback(callback: EvictionCallback);
                var options = GetDefaultCacheEntryOptions();
                result.SetOptions(options);
            }
            return result;
        }

        public virtual bool TryGetValue(object key, out object value)
        {
            return _memoryCache.TryGetValue(key, out value);
        }

        public virtual void Remove(object key)
        {
            _memoryCache.Remove(key);
        }


        protected bool CacheEnabled { get; set; }

        protected TimeSpan? AbsoluteExpiration { get; set; }

        protected TimeSpan? SlidingExpiration { get; set; }


        public virtual MemoryCacheEntryOptions GetDefaultCacheEntryOptions()
        {
            var result = new MemoryCacheEntryOptions();

            if (!CacheEnabled)
            {
                result.AbsoluteExpirationRelativeToNow = TimeSpan.FromTicks(1);
            }
            else
            {
                if (AbsoluteExpiration != null)
                {
                    result.AbsoluteExpirationRelativeToNow = AbsoluteExpiration;
                }
                else if (SlidingExpiration != null)
                {
                    result.SlidingExpiration = SlidingExpiration;
                }

                result.AddExpirationToken(GlobalCacheRegion.CreateChangeToken());
            }

            return result;
        }

        ~PlatformMemoryCache()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _memoryCache.Dispose();
                }
                _disposed = true;
            }
        }

        
        protected virtual void EvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            _log.LogTrace($"EvictionCallback: Cache entry with key:{key} has been removed.");
        }        
    }
}

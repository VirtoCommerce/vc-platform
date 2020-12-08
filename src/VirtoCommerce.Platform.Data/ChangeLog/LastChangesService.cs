using System;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.ChangeLog;

namespace VirtoCommerce.Platform.Data.ChangeLog
{
    public class LastChangesService : ILastChangesService
    {
        private readonly IPlatformMemoryCache _memoryCache;

        public LastChangesService(IPlatformMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public DateTimeOffset GetLastModified(string entityName)
        {
            DateTimeOffset result;

            entityName ??= string.Empty;

            var cacheKey = CacheKey.With(GetType(), "LastModifiedDateTime", entityName);
            result = _memoryCache.GetOrCreateExclusive(cacheKey, options =>
            {
                options.AddExpirationToken(LastChangesCacheRegion.CreateChangeTokenForKey(entityName));

                return DateTimeOffset.UtcNow;
            });

            return result;
        }

        public void Reset(string entityName)
        {
            entityName ??= string.Empty;

            LastChangesCacheRegion.ExpireTokenForKey(entityName);
        }
    }
}

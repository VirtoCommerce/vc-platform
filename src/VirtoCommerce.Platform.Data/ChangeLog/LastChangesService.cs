using System;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.ChangeLog
{
    public class LastChangesService : ILastChangesService
    {
        private readonly IPlatformMemoryCache _memoryCache;

        public LastChangesService(IPlatformMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public DateTimeOffset GetLastModifiedDate(string entityName)
        {
            entityName ??= string.Empty;

            var cacheKey = CacheKey.With(GetType(), nameof(GetLastModifiedDate), entityName);
            return _memoryCache.GetOrCreateExclusive(cacheKey, options =>
            {
                options.AddExpirationToken(LastChangesCacheRegion.CreateChangeTokenForKey(entityName));

                return DateTimeOffset.UtcNow;
            });
        }

        public void Reset(IEntity entity)
        {
            foreach (var entityTypeName in EntityTypeNames.Get(entity.GetType()))
            {
                Reset(entityTypeName);
            }
        }

        public void Reset(string entityName)
        {
            LastChangesCacheRegion.ExpireTokenForKey(entityName);
        }
    }
}

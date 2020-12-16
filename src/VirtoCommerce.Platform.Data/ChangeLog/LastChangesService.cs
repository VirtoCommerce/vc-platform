using System;
using System.Collections.Generic;
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
            var typeNames = new List<string>();

            var entityType = entity.GetType();

            while (entityType != null && entityType != typeof(Entity))
            {
                typeNames.Add(entityType.FullName);
                entityType = entityType.BaseType;
            }

            foreach (var entityTypeName in typeNames)
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

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

        public void Reset(AuditableEntity entity)
        {
            var typeNames = new List<string>();
            var entityType = entity.GetType();
            typeNames.Add(entityType.Name);
            while (!entityType.BaseType.Equals(typeof(AuditableEntity)))
            {
                entityType = entityType.BaseType;
                typeNames.Add(entityType.Name);
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

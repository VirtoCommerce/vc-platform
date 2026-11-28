using System;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Benchmark.ChangeLog;

// The LastChangesService implementation before VCST-7077, copied verbatim for before/after comparison.
// It uses its own cache region so its static per-key token registry does not mix with the current one.
public class LegacyLastChangesCacheRegion : CancellableCacheRegion<LegacyLastChangesCacheRegion>
{
}

public class LegacyLastChangesService : ILastChangesService
{
    private readonly IPlatformMemoryCache _memoryCache;

    public LegacyLastChangesService(IPlatformMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public DateTimeOffset GetLastModifiedDate(string entityName)
    {
        entityName ??= string.Empty;

        var cacheKey = CacheKey.With(GetType(), nameof(GetLastModifiedDate), entityName);
        return _memoryCache.GetOrCreateExclusive(cacheKey, options =>
        {
            options.AddExpirationToken(LegacyLastChangesCacheRegion.CreateChangeTokenForKey(entityName));

            return DateTimeOffset.UtcNow;
        });
    }

    public void Reset(IEntity entity)
    {
        // EntityTypeNames is internal to Platform.Data; the benchmarks only use Reset(string).
        Reset(entity.GetType().FullName);
    }

    public void Reset(string entityName)
    {
        LegacyLastChangesCacheRegion.ExpireTokenForKey(entityName);
    }
}

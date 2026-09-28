using System.Collections.Generic;
using System.Diagnostics.Metrics;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Caching;

internal static class CacheMetrics
{
    private static readonly Meter _meter = new("VirtoCommerce.Platform.Caching");
    private static readonly Counter<long> _requests = _meter.CreateCounter<long>(
        "virtocommerce.cache.requests", "{request}", "Cache lookups, including internal retries and in-flight entries.");

    public static void RecordLookup(bool hit, object key)
    {
        if (_requests.Enabled || CacheRequestMetrics.IsEnabled)
        {
            Record(hit, CacheKey.GetCacheName(key) ?? nameof(PlatformMemoryCache));
        }
    }

    public static void Record(bool hit, string cacheName)
    {
        CacheRequestMetrics.Record(hit, cacheName);
        _requests.Add(1,
            new KeyValuePair<string, object>("cache.request.type", hit ? "hit" : "miss"),
            new KeyValuePair<string, object>("cache.name", cacheName));
    }
}

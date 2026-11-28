using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Data.ChangeLog;

namespace VirtoCommerce.Platform.Benchmark.ChangeLog;

// One implementation of ILastChangesService under test (Legacy = before VCST-7077, Current = after),
// with the hooks the scenarios need: a Redis-style remote reset, a region reset and the static token count.
public sealed class LastChangesSubject
{
    public const string Legacy = "Legacy";
    public const string Current = "Current";

    private LastChangesSubject(string implementation, MemoryCache memoryCache, ILastChangesService service)
    {
        Implementation = implementation;
        MemoryCache = memoryCache;
        Service = service;
    }

    public string Implementation { get; }

    public MemoryCache MemoryCache { get; }

    public ILastChangesService Service { get; }

    public static LastChangesSubject Create(string implementation, TimeSpan? slidingExpiration = null)
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions { ExpirationScanFrequency = TimeSpan.FromMilliseconds(100) });
        var options = Options.Create(new CachingOptions { CacheEnabled = true, CacheSlidingExpiration = slidingExpiration ?? TimeSpan.FromMinutes(15) });
        var platformMemoryCache = new PlatformMemoryCache(memoryCache, options, NullLogger<PlatformMemoryCache>.Instance);

        ILastChangesService service = implementation switch
        {
            Legacy => new LegacyLastChangesService(platformMemoryCache),
            Current => new LastChangesService(platformMemoryCache),
            _ => throw new ArgumentOutOfRangeException(nameof(implementation)),
        };

        return new LastChangesSubject(implementation, memoryCache, service);
    }

    // What the Redis backplane does when another instance resets a name
    public void RemoteReset(string entityName)
    {
        var tokenKey = Implementation == Legacy
            ? LegacyLastChangesCacheRegion.GenerateRegionTokenKey(entityName)
            : LastChangesCacheRegion.GenerateRegionTokenKey(entityName);

        CancellableCacheRegion.CancelForKey(tokenKey, propagate: false);
    }

    public void ExpireRegion()
    {
        if (Implementation == Legacy)
        {
            LegacyLastChangesCacheRegion.ExpireRegion();
        }
        else
        {
            LastChangesCacheRegion.ExpireRegion();
        }
    }

    // Entries in the static per-key CancellationTokenSource registry of the implementation's region
    public int KeyTokenCount()
    {
        var regionType = Implementation == Legacy
            ? typeof(CancellableCacheRegion<LegacyLastChangesCacheRegion>)
            : typeof(CancellableCacheRegion<LastChangesCacheRegion>);
        var field = regionType.GetField("_keyTokensDict", BindingFlags.NonPublic | BindingFlags.Static)!;

        return ((ConcurrentDictionary<string, CancellationTokenSource>)field.GetValue(null))!.Count;
    }

    public static string[] UniqueNames(string prefix, int count)
    {
        return Enumerable.Range(0, count).Select(i => $"{prefix}-{i}").ToArray();
    }

    public static void WaitForClockTick()
    {
        var start = DateTimeOffset.UtcNow;
        while (DateTimeOffset.UtcNow == start)
        {
            Thread.Sleep(1);
        }
    }
}

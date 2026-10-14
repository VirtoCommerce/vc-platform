using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Caching;

internal static class CacheMetrics
{
    private static readonly Meter _meter = new("VirtoCommerce.Platform.Caching");
    private static readonly Counter<long> _requests = CreateCounter(
        "virtocommerce.cache.requests", "Cache lookups, including internal retries and in-flight entries.");
    private static readonly Counter<long> _requestGroups = CreateCounter(
        "virtocommerce.cache.request.groups", "Sampled requests by cache group and lookup outcome.");

    private static Counter<long> CreateCounter(string name, string description)
    {
        try
        {
            return _meter.CreateCounter<long>(name, "{request}", description);
        }
        catch (Exception)
        {
            // First use publishes the instrument to external listeners. A publication failure must
            // disable this counter, not poison the type initializer and every subsequent cache read.
            return null;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryGetRecorder(out CacheRequestMetrics request)
    {
        // Until the first sampled request, both Off and Meter need only static field reads.
        request = CacheRequestMetrics.HasStarted ? CacheRequestMetrics.Current : null;
        return _requests?.Enabled == true || request is not null;
    }

    public static void RecordLookup(bool hit, object key)
    {
        if (TryGetRecorder(out var request))
        {
            Record(hit ? 1 : 0, hit ? 0 : 1, CacheKey.GetCacheName(key) ?? nameof(PlatformMemoryCache), request);
        }
    }

    public static void Record(long hits, long misses, string cacheName, CacheRequestMetrics request)
    {
        request?.Record(hits, misses, cacheName);
        if (_requests?.Enabled == true)
        {
            if (hits != 0)
            {
                RecordLookups(hits, "hit", cacheName);
            }
            if (misses != 0)
            {
                RecordLookups(misses, "miss", cacheName);
            }
        }
    }

    private static void RecordLookups(long count, string result, string cacheName)
    {
        try
        {
            _requests.Add(count,
                new KeyValuePair<string, object>("cache.request.type", result),
                new KeyValuePair<string, object>("cache.name", cacheName));
        }
        catch (Exception)
        {
            // MeterListener callbacks run synchronously and are outside the cache's control.
            // A failing subscriber must not fail a cache read or strand a single-flight reservation.
        }
    }

    public static void RecordRequestGroup(string cacheName, long hits, long misses)
    {
        if (_requestGroups?.Enabled != true)
        {
            return;
        }

        try
        {
            string outcome;
            if (hits == 0)
            {
                outcome = "miss_only";
            }
            else
            {
                outcome = misses == 0 ? "hit_only" : "mixed";
            }
            _requestGroups.Add(1,
                new KeyValuePair<string, object>("cache.name", cacheName),
                new KeyValuePair<string, object>("cache.outcome", outcome));
        }
        catch (Exception)
        {
            // Completing telemetry must not replace the HTTP request's result or exception.
        }
    }
}

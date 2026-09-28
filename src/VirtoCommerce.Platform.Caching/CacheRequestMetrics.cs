using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace VirtoCommerce.Platform.Caching;

internal sealed class CacheRequestMetrics : IDisposable
{
    // Bound trace size even when a custom cache implementation supplies many group names.
    private const int MaxGroups = 64;
    private static readonly AsyncLocal<CacheRequestMetrics> _current = new();
    private readonly object _lock = new();
    private readonly Activity _activity;
    private readonly CacheRequestMetrics _previous;
    private readonly Dictionary<string, (long Hits, long Misses)> _groups = new(StringComparer.Ordinal);
    private long _hits;
    private long _misses;
    private bool _groupsTruncated;
    private bool _completed;

    private CacheRequestMetrics(Activity activity)
    {
        _activity = activity;
        _previous = _current.Value;
        _current.Value = this;
    }

    public static bool IsEnabled => _current.Value is not null;

    public static CacheRequestMetrics Begin(Activity activity)
    {
        return activity?.IsAllDataRequested == true ? new CacheRequestMetrics(activity) : null;
    }

    public static void Record(bool hit, string cacheName)
    {
        _current.Value?.RecordLookup(hit, cacheName);
    }

    private void RecordLookup(bool hit, string cacheName)
    {
        lock (_lock)
        {
            // AsyncLocal can flow into detached work. It must not change a completed request's totals.
            if (_completed)
            {
                return;
            }

            if (hit)
            {
                _hits++;
            }
            else
            {
                _misses++;
            }

            if (_groups.TryGetValue(cacheName, out var counts) || _groups.Count < MaxGroups)
            {
                _groups[cacheName] = (counts.Hits + (hit ? 1 : 0), counts.Misses + (hit ? 0 : 1));
            }
            else
            {
                _groupsTruncated = true;
            }
        }
    }

    public void Dispose()
    {
        lock (_lock)
        {
            if (_completed)
            {
                return;
            }

            _completed = true;
            _current.Value = _previous;
            _activity.SetTag("cache.hits", _hits);
            _activity.SetTag("cache.misses", _misses);

            foreach (var (name, counts) in _groups)
            {
                // One summary per group, not an event/span per lookup.
                _activity.AddEvent(new ActivityEvent("cache.lookup.summary", tags: new ActivityTagsCollection
                {
                    { "cache.name", name },
                    { "cache.hits", counts.Hits },
                    { "cache.misses", counts.Misses },
                }));
            }

            if (_groupsTruncated)
            {
                _activity.SetTag("cache.groups.truncated", true);
            }
        }
    }
}

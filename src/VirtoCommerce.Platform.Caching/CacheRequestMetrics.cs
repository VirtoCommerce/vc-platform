using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace VirtoCommerce.Platform.Caching;

internal sealed class CacheRequestMetrics : IDisposable
{
    private const int MaxGroups = 64;
    private const int MaxSummaryBytes = 8192;
    private static readonly AsyncLocal<CacheRequestMetrics> _current = new();
    private static volatile bool _hasStarted;
    private readonly Activity _activity;
    private readonly object _groupLock = new();
    private ConcurrentDictionary<string, Counts> _groups;
    private Counts _overflow;
    private int _groupCount;
    private int _completed;
    private int _writers;

    private CacheRequestMetrics(Activity activity)
    {
        _activity = activity;
        _current.Value = this;
    }

    public static bool HasStarted => _hasStarted;
    public static CacheRequestMetrics Current
    {
        get
        {
            var current = _current.Value;
            return current is not null && Volatile.Read(ref current._completed) == 0 ? current : null;
        }
    }

    public static CacheRequestMetrics Begin(Activity activity)
    {
        if (activity is not { IsAllDataRequested: true, Recorded: true })
        {
            return null;
        }

        _hasStarted = true;
        return new CacheRequestMetrics(activity);
    }

    public void Record(long hits, long misses, string cacheName)
    {
        if ((hits == 0 && misses == 0) || Volatile.Read(ref _completed) != 0)
        {
            return;
        }

        Interlocked.Increment(ref _writers);
        try
        {
            // Dispose closes admission before waiting for admitted writers. Detached work cannot
            // mutate the completed snapshot, and a lookup already being recorded is not lost.
            if (Volatile.Read(ref _completed) != 0)
            {
                return;
            }

            var groups = Volatile.Read(ref _groups);
            if (groups is null || !groups.TryGetValue(cacheName, out var counts))
            {
                // Once full, repeated unretained groups also stay off the monitor path.
                counts = Volatile.Read(ref _groupCount) == MaxGroups ? Volatile.Read(ref _overflow) : null;
                counts ??= GetOrAddGroup(cacheName);
            }

            counts.Add(hits, misses);
        }
        finally
        {
            Interlocked.Decrement(ref _writers);
        }
    }

    private Counts GetOrAddGroup(string cacheName)
    {
        // A lock is needed only when a group first appears, to enforce an exact bounded admission.
        // Existing groups are read and incremented without a monitor, including parallel resolvers.
        lock (_groupLock)
        {
            var groups = _groups;
            if (groups is null)
            {
                groups = new ConcurrentDictionary<string, Counts>(concurrencyLevel: 1, capacity: 3, comparer: StringComparer.Ordinal);
                Volatile.Write(ref _groups, groups);
            }
            if (groups.TryGetValue(cacheName, out var counts))
            {
                return counts;
            }
            if (_groupCount == MaxGroups)
            {
                return _overflow ??= new Counts();
            }
            counts = new Counts();
            groups.TryAdd(cacheName, counts);
            Volatile.Write(ref _groupCount, _groupCount + 1);
            return counts;
        }
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _completed, 1) != 0)
        {
            return;
        }

        var spin = new SpinWait();
        while (Volatile.Read(ref _writers) != 0)
        {
            spin.SpinOnce();
        }

        if (_groups is null)
        {
            return;
        }

        long hits = 0;
        long misses = 0;
        if (_overflow is not null)
        {
            hits = _overflow.Hits;
            misses = _overflow.Misses;
        }
        var truncated = hits != 0 || misses != 0;
        var buffer = new ArrayBufferWriter<byte>();
        using var writer = new Utf8JsonWriter(buffer);
        writer.WriteStartArray();
        foreach (var (name, counts) in _groups)
        {
            hits += counts.Hits;
            misses += counts.Misses;
            CacheMetrics.RecordRequestGroup(name, counts.Hits, counts.Misses);
            var encodedName = JsonEncodedText.Encode(name);
            // JSON punctuation, optional comma and closing outer bracket; the budget is UTF-8 bytes.
            var entryBytes = encodedName.EncodedUtf8Bytes.Length + NumberLength(counts.Hits) + NumberLength(counts.Misses) + 7;
            if (writer.BytesCommitted + writer.BytesPending + entryBytes + 1 > MaxSummaryBytes)
            {
                truncated = true;
                continue;
            }
            writer.WriteStartArray();
            writer.WriteStringValue(encodedName);
            writer.WriteNumberValue(counts.Hits);
            writer.WriteNumberValue(counts.Misses);
            writer.WriteEndArray();
        }
        writer.WriteEndArray();
        writer.Flush();

        _activity.SetTag("cache.hits", hits);
        _activity.SetTag("cache.misses", misses);
        _activity.SetTag("cache.lookup.summary", Encoding.UTF8.GetString(buffer.WrittenSpan));
        if (truncated)
        {
            _activity.SetTag("cache.groups.truncated", true);
        }
    }

    private static int NumberLength(long value)
    {
        Span<char> digits = stackalloc char[20];
        value.TryFormat(digits, out var written, provider: CultureInfo.InvariantCulture);
        return written;
    }

    private sealed class Counts
    {
        public long Hits;
        public long Misses;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(long hits, long misses)
        {
            if (hits != 0)
            {
                Interlocked.Add(ref Hits, hits);
            }
            if (misses != 0)
            {
                Interlocked.Add(ref Misses, misses);
            }
        }
    }
}

using System;
using System.Buffers;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;

namespace VirtoCommerce.Platform.Caching;

internal sealed class CacheRequestMetrics : IDisposable
{
    private const int MaxGroups = 64;
    private const int GroupsPerBlock = 4;
    private const int MaxSummaryBytes = 8192;
    private static readonly AsyncLocal<CacheRequestMetrics> _current = new();
    private static volatile bool _hasStarted;
    private readonly Activity _activity;
    private GroupBlock _groups;
    private long _overflowHits;
    private long _overflowMisses;
    private int _completed;

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
        if (activity is not { IsAllDataRequested: true, Recorded: true } || !activity.Source.HasListeners())
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

        var block = GetOrCreateBlock(ref _groups);
        for (var index = 0; index < MaxGroups / GroupsPerBlock; index++)
        {
            if (block.TryRecord(cacheName, hits, misses))
            {
                return;
            }
            if (index + 1 < MaxGroups / GroupsPerBlock)
            {
                block = GetOrCreateBlock(ref block.Next);
            }
        }
        Add(ref _overflowHits, ref _overflowMisses, hits, misses);
    }

    private static GroupBlock GetOrCreateBlock(ref GroupBlock location)
    {
        var block = Volatile.Read(ref location);
        if (block is null)
        {
            var candidate = new GroupBlock();
            block = Interlocked.CompareExchange(ref location, candidate, null) ?? candidate;
        }
        return block;
    }

    public void Dispose()
    {
        if (Interlocked.Exchange(ref _completed, 1) != 0 || _groups is null)
        {
            return;
        }

        // Do not drain detached lookups. Read each counter once and use that same value for totals,
        // JSON and outcomes. In-flight work may be omitted; it cannot change the published snapshot.
        var buffer = ArrayPool<char>.Shared.Rent(MaxSummaryBytes);
        try
        {
            var snapshot = WriteSnapshot(buffer);
            if (snapshot.Hits == 0 && snapshot.Misses == 0)
            {
                return;
            }
            _activity.SetTag("cache.hits", snapshot.Hits);
            _activity.SetTag("cache.misses", snapshot.Misses);
            _activity.SetTag("cache.lookup.summary", new string(buffer, 0, snapshot.Written));
            if (snapshot.Truncated)
            {
                _activity.SetTag("cache.groups.truncated", true);
            }
        }
        finally
        {
            ArrayPool<char>.Shared.Return(buffer);
        }
    }

    private Snapshot WriteSnapshot(Span<char> buffer)
    {
        var snapshot = new Snapshot
        {
            Hits = Volatile.Read(ref _overflowHits),
            Misses = Volatile.Read(ref _overflowMisses),
            Written = 1,
            Bytes = 2, // Both outer brackets.
        };
        snapshot.Truncated = snapshot.Hits != 0 || snapshot.Misses != 0;
        buffer[0] = '[';
        for (var block = Volatile.Read(ref _groups); block is not null; block = Volatile.Read(ref block.Next))
        {
            for (var index = 0; index < GroupsPerBlock; index++)
            {
                ref var group = ref block.Groups[index];
                var name = Volatile.Read(ref group.Name);
                var hits = Volatile.Read(ref group.Hits);
                var misses = Volatile.Read(ref group.Misses);
                if (name is not null && (hits != 0 || misses != 0))
                {
                    snapshot.Hits += hits;
                    snapshot.Misses += misses;
                    AppendGroup(buffer, ref snapshot, name, hits, misses);
                    CacheMetrics.RecordRequestGroup(name, hits, misses);
                }
            }
        }
        buffer[snapshot.Written++] = ']';
        return snapshot;
    }

    private static void AppendGroup(Span<char> buffer, ref Snapshot snapshot, string name, long hits, long misses)
    {
        var comma = snapshot.Written > 1 ? 1 : 0;
        var remaining = MaxSummaryBytes - snapshot.Bytes - comma;
        var target = buffer.Slice(snapshot.Written + comma, Math.Max(0, remaining));
        if (!TryWriteGroup(target, name, hits, misses, out var written))
        {
            snapshot.Truncated = true;
            return;
        }
        var bytes = Encoding.UTF8.GetByteCount(target[..written]);
        if (bytes > remaining)
        {
            snapshot.Truncated = true;
            return;
        }
        if (comma != 0)
        {
            buffer[snapshot.Written] = ',';
        }
        snapshot.Written += comma + written;
        snapshot.Bytes += comma + bytes;
    }

    private static bool TryWriteGroup(Span<char> destination, string name, long hits, long misses, out int written)
    {
        const int minimumMissesSuffixLength = 3; // Comma, at least one digit, closing bracket.
        written = 0;
        if (destination.Length < 8)
        {
            return false;
        }
        destination[0] = '[';
        destination[1] = '"';
        var status = JavaScriptEncoder.Default.Encode(name.AsSpan(), destination[2..^6], out _, out var encoded);
        if (status != OperationStatus.Done)
        {
            return false;
        }
        var position = encoded + 2;
        destination[position++] = '"';
        destination[position++] = ',';
        if (!hits.TryFormat(destination[position..^minimumMissesSuffixLength], out var digits, provider: CultureInfo.InvariantCulture))
        {
            return false;
        }
        position += digits;
        destination[position++] = ',';
        if (!misses.TryFormat(destination[position..^1], out digits, provider: CultureInfo.InvariantCulture))
        {
            return false;
        }
        position += digits;
        destination[position++] = ']';
        written = position;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void Add(ref long targetHits, ref long targetMisses, long hits, long misses)
    {
        if (hits != 0)
        {
            Interlocked.Add(ref targetHits, hits);
        }
        if (misses != 0)
        {
            Interlocked.Add(ref targetMisses, misses);
        }
    }

    // Allocate four slots at a time, instead of paying for all 64 on a common 1-3 group request.
    // Names are published once in slot order. Losing a CAS rechecks that slot before advancing,
    // so racing callers cannot admit duplicate names or exceed the fixed block/slot bound.
    private sealed class GroupBlock
    {
        public readonly Group[] Groups = new Group[GroupsPerBlock];
        public GroupBlock Next;

        public bool TryRecord(string name, long hits, long misses)
        {
            for (var index = 0; index < GroupsPerBlock; index++)
            {
                ref var group = ref Groups[index];
                var existing = Volatile.Read(ref group.Name);
                if (existing is null)
                {
                    existing = Interlocked.CompareExchange(ref group.Name, name, null) ?? name;
                }
                if (string.Equals(existing, name, StringComparison.Ordinal))
                {
                    Add(ref group.Hits, ref group.Misses, hits, misses);
                    return true;
                }
            }
            return false;
        }
    }

    private struct Group
    {
        public string Name;
        public long Hits;
        public long Misses;
    }

    private struct Snapshot
    {
        public long Hits;
        public long Misses;
        public int Written;
        public int Bytes;
        public bool Truncated;
    }
}

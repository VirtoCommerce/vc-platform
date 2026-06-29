using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using BenchmarkDotNet.Attributes;

namespace VirtoCommerce.Platform.Benchmark.AbstractTypeFactory;

/// <summary>
/// Measures the overhead of thread-safe patterns vs non-thread-safe equivalents.
/// Isolates: Dictionary vs ConcurrentDictionary, direct read vs Volatile.Read.
/// </summary>
[MemoryDiagnoser]
[SimpleJob]
public class ThreadSafetyOverheadBenchmarks
{
    private static readonly Func<BaseEntity> _factory =
        Expression.Lambda<Func<BaseEntity>>(Expression.New(typeof(DerivedEntity))).Compile();

    // Non-thread-safe
    private static readonly Dictionary<string, Func<BaseEntity>> _dict = new(StringComparer.OrdinalIgnoreCase);

    // Thread-safe
    private static readonly ConcurrentDictionary<string, Func<BaseEntity>> _concurrentDict = new(StringComparer.OrdinalIgnoreCase);

    // For Volatile.Read vs direct read
    private static Func<BaseEntity> _cachedFactory;

    private static readonly string _typeName = nameof(BaseEntity);

    [GlobalSetup]
    public void Setup()
    {
        _dict[_typeName] = _factory;
        _concurrentDict[_typeName] = _factory;
        _cachedFactory = _factory;
    }

    // ── Dictionary lookup + delegate ────────────────────────────

    [Benchmark(Baseline = true)]
    public BaseEntity Dict_TryGetValue()
    {
        return _dict.TryGetValue(_typeName, out var factory)
            ? factory()
            : throw new InvalidOperationException();
    }

    [Benchmark]
    public BaseEntity ConcurrentDict_TryGetValue()
    {
        return _concurrentDict.TryGetValue(_typeName, out var factory)
            ? factory()
            : throw new InvalidOperationException();
    }

    // ── Cached delegate read ────────────────────────────────────

    [Benchmark]
    public BaseEntity DirectRead_Delegate()
    {
        return _cachedFactory();
    }

    [Benchmark]
    public BaseEntity VolatileRead_Delegate()
    {
        return Volatile.Read(ref _cachedFactory)();
    }
}

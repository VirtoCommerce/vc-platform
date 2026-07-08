using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Benchmark.ValueObjects;

// A representative ValueObject subtype that does NOT override GetEqualityComponents, so it exercises
// the reflective base implementation (the hot path this benchmark targets). It mixes reference-typed,
// value-typed, nullable, and list-typed (the IsAssignableFromGenericList branch) properties, plus a
// null-valued property on half the instances (the null branch), mirroring real subtypes such as
// search criteria and index document fields.
public sealed class BenchValueObject : ValueObject
{
    public string Name { get; set; }
    public string Category { get; set; }
    public int Rank { get; set; }
    public int? OptionalRank { get; set; }
    public bool IsActive { get; set; }
    public List<string> Tags { get; set; }
}

// Measures the cost of structural equality / hashing / cache-key computation over a large collection
// of a ValueObject subtype. Each operation goes through GetEqualityComponents, which (before the fix)
// reflects over every property via PropertyInfo.GetValue on every call.
//
// Allocated is the trustworthy, machine-portable signal: the per-property GetValue boxes through
// reflection and allocates an internal argument array on every call. Time Mean is comparable only
// within a single controlled run (e.g. a git-switched before/after on the same host).
[MemoryDiagnoser]
public class ValueObjectBenchmarks
{
    [Params(1000, 10000)]
    public int Count { get; set; }

    private List<BenchValueObject> _items;
    private List<BenchValueObject> _twins;

    [GlobalSetup]
    public void Setup()
    {
        _items = new List<BenchValueObject>(Count);
        _twins = new List<BenchValueObject>(Count);

        for (var i = 0; i < Count; i++)
        {
            _items.Add(Create(i));
            // Distinct value-equal instances (different references): forces Equals to enumerate all
            // components on BOTH operands — the worst case for the reflective path.
            _twins.Add(Create(i));
        }
    }

    private static BenchValueObject Create(int i)
    {
        return new BenchValueObject
        {
            Name = "name-" + i,
            Category = "cat-" + (i % 50),
            Rank = i,
            OptionalRank = i % 3 == 0 ? null : i,
            IsActive = i % 2 == 0,
            // Half the instances leave Tags null (the null branch); the rest hit the list branch.
            Tags = i % 2 == 0 ? null : ["a" + i, "b" + i],
        };
    }

    [Benchmark]
    public int HashCode_Sum()
    {
        var sum = 0;
        foreach (var item in _items)
        {
            sum += item.GetHashCode();
        }

        return sum;
    }

    [Benchmark]
    public int Distinct_Count()
    {
        return _items.Distinct().Count();
    }

    [Benchmark]
    public int Equals_AllValueEqualTwins()
    {
        var equalCount = 0;
        for (var i = 0; i < _items.Count; i++)
        {
            if (_items[i].Equals(_twins[i]))
            {
                equalCount++;
            }
        }

        return equalCount;
    }

    [Benchmark]
    public long CacheKey_TotalLength()
    {
        long total = 0;
        foreach (var item in _items)
        {
            total += item.GetCacheKey().Length;
        }

        return total;
    }
}

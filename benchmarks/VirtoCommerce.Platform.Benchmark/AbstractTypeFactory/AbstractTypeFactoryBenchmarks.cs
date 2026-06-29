using System;
using System.Linq.Expressions;
using BenchmarkDotNet.Attributes;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Benchmark.AbstractTypeFactory;

// ── Domain types ────────────────────────────────────────────────

public class BaseEntity
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class DerivedEntity : BaseEntity
{
    public string ExtraProperty { get; set; }
}

public class ParameterizedEntity : BaseEntity
{
    public ParameterizedEntity(string id, string name)
    {
        Id = id;
        Name = name;
    }
}

// ── Benchmarks ──────────────────────────────────────────────────

[MemoryDiagnoser]
[SimpleJob]
public class AbstractTypeFactoryBenchmarks
{
    private static readonly string _id = "test-id";
    private static readonly string _name = "test-name";

    [GlobalSetup]
    public void Setup()
    {
        // Register DerivedEntity as override of BaseEntity
        AbstractTypeFactory<BaseEntity>.RegisterType<DerivedEntity>();

        // Register ParameterizedEntity for args tests
        AbstractTypeFactory<ParameterizedEntity>.RegisterType<ParameterizedEntity>();
    }

    // ── Baselines ──────────────────────────────────────────────

    [Benchmark(Baseline = true)]
    public BaseEntity DirectNew()
    {
        return new DerivedEntity();
    }

    [Benchmark]
    public BaseEntity Activator_Parameterless()
    {
        return (BaseEntity)Activator.CreateInstance(typeof(DerivedEntity));
    }

    [Benchmark]
    public ParameterizedEntity Activator_WithArgs()
    {
        return (ParameterizedEntity)Activator.CreateInstance(typeof(ParameterizedEntity), _id, _name);
    }

    // ── Optimized AbstractTypeFactory (real code) ───────────────

    [Benchmark]
    public BaseEntity Factory_TryCreateInstance()
    {
        return AbstractTypeFactory<BaseEntity>.TryCreateInstance();
    }

    [Benchmark]
    public CleanEntity Factory_TryCreateInstance_NoOverrides()
    {
        // CleanEntity has NO registrations → fast-path via _typeInfos.Count == 0
        return AbstractTypeFactory<CleanEntity>.TryCreateInstance();
    }

    [Benchmark]
    public ParameterizedEntity Factory_TryCreateInstance_WithArgs()
    {
        return AbstractTypeFactory<ParameterizedEntity>.TryCreateInstance(nameof(ParameterizedEntity), _id, _name);
    }

    [Benchmark]
    public BaseEntity Factory_TryCreateInstance_ByDerivedName()
    {
        return AbstractTypeFactory<BaseEntity>.TryCreateInstance(nameof(DerivedEntity));
    }

    // ── Pure delegate call (theoretical floor) ──────────────────

    private static readonly Func<BaseEntity> _compiledDelegate =
        Expression.Lambda<Func<BaseEntity>>(Expression.New(typeof(DerivedEntity))).Compile();

    [Benchmark]
    public BaseEntity Delegate_Pure()
    {
        return _compiledDelegate();
    }
}

// Separate type with zero factory registrations
public class CleanEntity
{
    public string Id { get; set; }
}

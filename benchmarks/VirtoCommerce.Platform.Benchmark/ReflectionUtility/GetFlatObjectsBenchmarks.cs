using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Benchmark.ReflectionUtility;

// A representative entity-graph node: scalar props (exercise the "GetValue on value-typed props"
// path), a List<string> (non-T enumerable — iterated but its string elements are not IEntity),
// and a List<BenchEntity> (the T-bearing collection actually recursed into).
public class BenchEntity : Entity
{
    public string Name { get; set; }
    public string Code { get; set; }
    public int Number { get; set; }
    public bool Flag { get; set; }
    public List<string> Tags { get; set; }
    public List<BenchEntity> Children { get; set; }
}

// Measures GetFlatObjectsListWithInterface<IEntity>() over an entity tree — the shape used on
// catalog/order/cart save+load paths to collect IEntity / IHasImages / IOperation nodes. Before any
// optimization this reflects over GetProperties + GetValue per visited node on every call.
[MemoryDiagnoser]
public class GetFlatObjectsBenchmarks
{
    [Params(50, 500)]
    public int NodeCount { get; set; }

    private BenchEntity _root;

    [GlobalSetup]
    public void Setup()
    {
        _root = new BenchEntity
        {
            Id = "root",
            Name = "r",
            Code = "c",
            Tags = ["a", "b"],
            Children = [],
        };

        var remaining = NodeCount - 1;
        var queue = new Queue<BenchEntity>();
        queue.Enqueue(_root);
        var index = 0;

        while (remaining > 0 && queue.Count > 0)
        {
            var parent = queue.Dequeue();
            for (var i = 0; i < 5 && remaining > 0; i++)
            {
                var child = new BenchEntity
                {
                    Id = "n" + index,
                    Name = "name-" + index,
                    Code = "code-" + index,
                    Number = index,
                    Flag = index % 2 == 0,
                    Tags = ["x" + index, "y" + index],
                    Children = [],
                };
                parent.Children.Add(child);
                queue.Enqueue(child);
                remaining--;
                index++;
            }
        }
    }

    [Benchmark]
    public int FlattenEntities()
    {
        return _root.GetFlatObjectsListWithInterface<IEntity>().Length;
    }
}

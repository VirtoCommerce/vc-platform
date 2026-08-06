using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Benchmark.Caching;

// What a cache MISS pays to build its key: serialize the argument graph and hash it. That cost is charged
// on every call, so on a request that never hits it is pure overhead — which is the number a caller needs
// before deciding the cache is free.
//
// The payload is a synthetic polymorphic tree rather than a real search request: Platform.Core cannot
// reference a search module, and the property that matters here is shape and size, not domain meaning —
// an interface-typed tree with string leaves, sized by NodeCount to bracket a few-KB request.
[MemoryDiagnoser]
public class JsonHashBenchmarks
{
    // The shipped settings, so the two arms serialize identically and the comparison stays honest.
    private static readonly JsonSerializerSettings _settings = JsonHashExtensions.CreateCacheKeySettings();

    [Params(8, 64, 256)]
    public int NodeCount { get; set; }

    private Holder _payload;

    [GlobalSetup]
    public void Setup()
    {
        _payload = new Holder
        {
            Root = new AndNode
            {
                Children = Enumerable.Range(0, NodeCount)
                    .Select(i => (INode)new Leaf { Value = $"field-{i}:{new string('v', 24)}" })
                    .ToList(),
            },
        };
    }

    // Materialized is the baseline: it is the obvious implementation the streaming writer replaces, so the
    // ratio column reads as "what the streaming writer bought".
    [Benchmark(Baseline = true)]
    public string Materialized()
    {
        // Omit the declared root type and this arm writes no root $type, so the two arms hash DIFFERENT
        // payloads: the ratio charges Streamed for bytes the baseline never wrote, worst at the smallest
        // NodeCount. Nothing catches that - it compiles, it runs, and no test asserts the arms agree.
        var json = JsonConvert.SerializeObject(_payload, typeof(object), _settings);

        return Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(json)));
    }

    [Benchmark]
    public string Streamed() => _payload.GetJsonSha256Hex();

    public class Holder
    {
        public INode Root { get; set; }
    }

    public interface INode
    {
    }

    public class AndNode : INode
    {
        public IList<INode> Children { get; set; }
    }

    public class Leaf : INode
    {
        public string Value { get; set; }
    }
}

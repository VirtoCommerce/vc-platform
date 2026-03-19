using BenchmarkDotNet.Running;
using VirtoCommerce.Platform.Benchmark.AbstractTypeFactory;

BenchmarkSwitcher.FromAssembly(typeof(AbstractTypeFactoryBenchmarks).Assembly).Run(args);

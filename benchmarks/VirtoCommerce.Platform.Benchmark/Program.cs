using BenchmarkDotNet.Running;
using VirtoCommerce.Platform.Benchmark.ValueObjects;

// Discovers every [Benchmark] class in this assembly. Select with --filter and choose a job with --job:
//   dotnet run -c Release -- --filter '*ValueObject*'
//   dotnet run -c Release -- --filter '*' --job short
BenchmarkSwitcher.FromAssembly(typeof(ValueObjectBenchmarks).Assembly).Run(args);

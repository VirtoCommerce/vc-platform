using BenchmarkDotNet.Running;
using VirtoCommerce.Platform.Benchmark.ChangeLog;
using VirtoCommerce.Platform.Benchmark.ValueObjects;

// Plain console runs that are not BenchmarkDotNet benchmarks
switch (args.Length > 0 ? args[0] : null)
{
    case "lastchanges-regression":
        LastChangesRegression.Run();
        return;
    case "lastchanges-retained":
        LastChangesRetainedMemory.Run(args[1], int.Parse(args[2]));
        return;
}

// Discovers every [Benchmark] class in this assembly. Select with --filter and choose a job with --job:
//   dotnet run -c Release -- --filter '*ValueObject*'
//   dotnet run -c Release -- --filter '*' --job short
BenchmarkSwitcher.FromAssembly(typeof(ValueObjectBenchmarks).Assembly).Run(args);

# AbstractTypeFactory Benchmarks

Measures the performance of `AbstractTypeFactory<T>` optimizations against baselines
(direct `new`, `Activator.CreateInstance`, raw compiled delegates).

## Prerequisites

- .NET 10 SDK (or whichever TFM the project currently targets)

## Running All Benchmarks

```bash
cd tests/VirtoCommerce.Platform.Benchmark.AbstractTypeFactory
dotnet run -c Release -- --filter '*'
```

## Running a Specific Benchmark Class

```bash
dotnet run -c Release -- --filter '*AbstractTypeFactoryBenchmarks*'
dotnet run -c Release -- --filter '*ThreadSafetyOverheadBenchmarks*'
```

## Running a Specific Benchmark Method

```bash
dotnet run -c Release -- --filter '*Factory_TryCreateInstance'
```

## Comparing Across .NET Versions

To compare performance between .NET versions (e.g., net10.0 vs net11.0):

1. Check out the tag/commit for the old .NET version.
2. Run benchmarks and save results: `dotnet run -c Release -- --filter '*' --artifacts ./results-net10`
3. Check out the current branch with the new .NET version.
4. Run benchmarks and save results: `dotnet run -c Release -- --filter '*' --artifacts ./results-net11`
5. Compare the two `BenchmarkDotNet.Artifacts/` output directories.

Results are written to `BenchmarkDotNet.Artifacts/` by default.

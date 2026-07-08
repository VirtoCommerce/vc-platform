# VirtoCommerce.Platform.Benchmark

BenchmarkDotNet micro-benchmarks for `VirtoCommerce.Platform.Core` primitives. Consolidates what used
to be several separate benchmark projects into one.

## Layout

Each benchmark suite lives in its own subfolder with a matching `VirtoCommerce.Platform.Benchmark.<Suite>`
namespace (e.g. `AbstractTypeFactory/`, `ArrayVsList/`, `ValueObjects/`, `ReflectionUtility/`). To see the
suites and benchmarks that actually exist, ask the runner rather than trusting a hand-maintained list:

```bash
cd benchmarks/VirtoCommerce.Platform.Benchmark
dotnet run -c Release -- --list tree
```

## Prerequisites

- .NET 10 SDK (or whichever TFM the project currently targets).

## Running

```bash
cd benchmarks/VirtoCommerce.Platform.Benchmark

# All benchmarks
dotnet run -c Release -- --filter '*'

# A suite or class (glob over the fully-qualified name)
dotnet run -c Release -- --filter '*ValueObject*'

# A specific method
dotnet run -c Release -- --filter '*ValueObjectBenchmarks.HashCode_Sum'
```

## Choosing a job

`Allocated` (the `[MemoryDiagnoser]` column) is exact even on a cheap job, so for an
allocation-focused check a quick job suffices; a trustworthy time `Mean` needs the full default job.

```bash
dotnet run -c Release -- --filter '*ValueObject*' --job dry     # gross alloc in seconds
dotnet run -c Release -- --filter '*ValueObject*' --job short   # byte-accurate alloc, cheaper than default
dotnet run -c Release -- --filter '*ValueObject*'               # default job — trustworthy time Mean
```

## Comparing before/after a change

The benchmark project references `VirtoCommerce.Platform.Core` by source, so a before/after comparison
is a git-switched two-run on the same host:

1. On the baseline revision: `dotnet run -c Release -- --filter '*ValueObject*' --artifacts ./before`
2. On the changed branch: `dotnet run -c Release -- --filter '*ValueObject*' --artifacts ./after`
3. Compare the `Allocated` / `Mean` columns in the two `BenchmarkDotNet.Artifacts` outputs.

Results are written to `BenchmarkDotNet.Artifacts/` by default.

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

Use a warmed short job for an allocation-focused check. Dry is a harness smoke check and can
include one-off initialization allocations; a trustworthy time `Mean` needs the full default job.

```bash
dotnet run -c Release -- --filter '*ValueObject*' --job dry     # harness smoke in seconds
dotnet run -c Release -- --filter '*ValueObject*' --job short   # warmed allocation check, cheaper than default
dotnet run -c Release -- --filter '*ValueObject*'               # default job — trustworthy time Mean
```

## Comparing before/after a change

The benchmark project references `VirtoCommerce.Platform.Core` by source, so a before/after comparison
is a git-switched two-run on the same host:

1. On the baseline revision: `dotnet run -c Release -- --filter '*ValueObject*' --artifacts ./before`
2. On the changed branch: `dotnet run -c Release -- --filter '*ValueObject*' --artifacts ./after`
3. Compare the `Allocated` / `Mean` columns in the two `BenchmarkDotNet.Artifacts` outputs.

Results are written to `BenchmarkDotNet.Artifacts/` by default.

## Cache telemetry regression matrix

`Caching/CacheMetricsBenchmarks` measures typed/untyped memory hits, warm exclusive and batch
helpers, request-scoped hits/batches, key creation and transient CRUD construction in `Off`,
`OffAfterRequest`, `Meter` and `Request` states. `OffAfterRequest` completes a sampled scope during
setup, then measures with no listeners and no current scope: the process-wide `HasStarted` flag
remains set. `Meter` uses a no-op `MeterListener` to isolate instrumentation cost; it does
not measure an exporter's aggregation or network cost. `Request` adds a sampled accumulator when
that API exists. On a pre-instrumentation baseline, the same source runs without an accumulator.
Reflection, listener creation, factories and warming are outside the measured operations.

The parallel methods use eight `Parallel.For` partitions with maximum degree eight; scheduling is
included and may use fewer threads. `CacheMetricsContentionBenchmarks` supplements these with eight
fixed threads sharing one request, amortizing barriers over 32,768 operations per invocation.

```console
dotnet run -c Release -- --filter '*CacheMetrics*Benchmarks*' --job dry --exporters json --artifacts ./smoke
dotnet run -c Release -- --filter '*CacheMetrics*Benchmarks*' --exporters json --artifacts ./measured
```

Compare identical benchmark sources and project references on baseline and branch, on the same
machine/runtime in Release, sequentially without competing builds or tests. Retain full JSON and
report Mean/Error/StdDev, allocations and monitor contentions. Dry is a harness smoke check, not a
timing comparison; small amortized allocation differences also need a warmed job. Separate scheduler
or barrier cost from instrumentation cost by comparing the same scenario on every revision.

`CacheMetricsRequestBenchmarks` includes the complete middleware lifecycle (Activity/context,
accumulator initialization, lookups and completion), for true Off, OffAfterRequest, RecordOnly and
Recorded requests with zero or three groups. Each has synchronous and asynchronous downstream cases.
The asynchronous case returns an incomplete task from the handler, then completes it after middleware
invocation has returned. This forces the middleware's await suspension and async-box allocation without
adding thread-pool scheduling or I/O latency. The completion source is included equally in the baseline.
Tracing uses a real ActivitySource and listener; Off has neither a tracing nor a meter listener.
This exposes costs excluded by the warmed lookup suites, including empty-request allocation,
request grouping and summary serialization.

`CacheGroupScaleBenchmarks` supplements the common one-group hot path with 1, 16 and 64 groups
in one sampled scope. It uses pre-normalized warm keys and visits each group once per invocation;
the reported time is per pass, so divide by `Groups` for per-lookup comparisons. The separate
ShortRun diagnostic in the report includes its wider confidence intervals and does not replace
the main Default job:

```console
dotnet run -c Release -- --filter '*CacheGroupScaleBenchmarks*' --job short --exporters json
```

The [2026-10-01 comparison](Caching/CacheMetricsResults.md) records base, reviewed and fixed
measurements with timing dispersion, allocation and contention diagnostics.

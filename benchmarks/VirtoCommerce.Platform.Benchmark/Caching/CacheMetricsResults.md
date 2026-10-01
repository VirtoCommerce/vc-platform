# Cache telemetry benchmark comparison — repeat review, 2026-10-01

- **Base:** `658d74c4247724cb0d260360fc159ef1e67a40d3` (before instrumentation).
- **Published reference:** `28f7c1c762c2478f90ff8080b358a4557dec1c00`.
- **Fixed:** the implementation accompanying this report, based on the published reference above.
- Windows 11 25H2, AMD Ryzen 7 8845HS (8 cores / 16 logical processors), SDK 10.0.400,
  runtime 10.0.11, BenchmarkDotNet 0.15.8, Release, default warmed job.

## Method

All 192 cases completed (64 per revision): 40 warm-operation cases, 8 fixed-thread contention cases,
16 complete-request cases. Identical benchmark sources/project references were built on all revisions.
Runs were sequential, without concurrent builds or tests. Full JSON retains statistics and diagnostics.

Off has no meter or tracing listener. OffAfterRequest first completes a sampled scope during setup,
then removes listeners; HasStarted remains true, with Current verified null. Meter uses a no-op
measurement callback, and Request adds a sampled accumulator through a real ActivitySource listener.
Base has no accumulator. Reflection is used only in setup. These measure platform instrumentation,
not exporter aggregation, network cost or production throughput.

The subsequent Sonar cleanup names a JSON suffix-length constant and adjusts test-only cleanup/assertions.
The compiled `TryWriteGroup` IL is byte-identical before and after that product change (278 bytes,
SHA-256 `2587D8C4DC91A7AAA3A2A078307C4522A9F95FFEAECDAA2925D974E96A92614D`),
so the measured runtime implementation is unchanged.

Complete-request cases cover Off / OffAfterRequest / RecordOnly / Recorded, zero/three groups,
and synchronous/suspended downstream tasks. The suspended case returns an incomplete task and completes
it after middleware invocation returns, forcing the middleware's async box without thread-pool/I/O
latency. Its completion source, Activity and context costs are included equally on every revision.

Time is per operation; batch methods are per 50-ID batch. Main parallel cases use eight Parallel.For
partitions (not guaranteed eight active threads); the contention suite uses eight fixed threads and
amortizes barriers over 32,768 lookups. Small parallel allocation/contention differences may include
that harness. Within-run confidence intervals do not capture all between-process/JIT/clock variation.

This harness differs from the earlier legacy-Activity/synchronous-only harness. Do not directly mix
its ns/B values with the [earlier published report](https://github.com/VirtoCommerce/vc-platform/blob/28f7c1c762c2478f90ff8080b358a4557dec1c00/benchmarks/VirtoCommerce.Platform.Benchmark/Caching/CacheMetricsResults.md).
The original 13-review-finding comparison remains available there.

Normalized SHA-256 (LF, one final newline), identical across all three checkouts:
- `CacheMetricsBenchmarks.cs`: `108FD76B799031AA08C8C40F312FA6F54FA726EA1B3525FD3227417C5207E4E7`
- `CacheMetricsContentionBenchmarks.cs`: `E41B5950590C03D4E1596F2806C94F5AB9FAA2E949896F58BBDA93135366267C`
- `CacheMetricsRequestBenchmarks.cs`: `B7F5BEEA773CC4FE625F0CD4913F0173F759DD83EF7FB415639F9DDC321BF290`

## Findings

- Times and allocations below are **base / published 28f7c1c7 / fixed implementation**, using the expanded, identical harness on each revision.
- Warm Off and OffAfterRequest: 20 of 20 cases have exactly baseline allocated bytes. Scalar Off/OffAfterRequest cases with nonzero monitor contentions: 0. Parallel diagnostics include the common scheduling/barrier harness; see every row below.
- Complete Off, OffAfterRequest and RecordOnly requests: 12 of 12 cases have baseline allocations, including genuinely suspended downstream tasks.
- Complete sampled request, three groups, synchronous downstream: **380.74 / 1042.62 / 676.23 ns**, **1608 / 3280 / 2232 B**. Suspended downstream: **383.76 / 1152.07 / 756.15 ns**, **1696 / 3496 / 2448 B**.
- Eight fixed threads sharing one sampled request, typed hit: **20.36 / 62.54 / 23.17 ns/op**, **88 / 88 / 88 B/op**. The shared writers counter and completion drain are removed; per-group atomic increments remain.
- Request by-key Off: **12.03 / 12.26 / 12.94 ns**, **0 / 0 / 0 B**; after a sampled request has set HasStarted: **12.10 / 13.69 / 13.59 ns**, **0 / 0 / 0 B**.
- Empty sampled request: **222.52 / 281.26 / 276.14 ns**, **1464 / 1640 / 1608 B**. A scope still has a cost even when it emits no tags. Single Meter typed hit: **50.59 / 88.34 / 82.19 ns**; Off CRUD constructor: **30.50 / 35.70 / 34.70 ns**. Instrumentation is not free and no universal or production-throughput speedup is claimed.
- Lookups completed before request completion remain included. A lookup racing completion may be omitted. Each counter is read once into the snapshot used by totals, JSON and group outcomes; detached work cannot change published attributes. This sampled attribution contract avoids waiting for in-flight work and does not change the physical lookup counter.
- The bounded linear slot search trades some upper-bound CPU for smaller common-request structures. The separate ShortRun diagnostic below measures 16 groups at 113.51 / 115.95 ns per lookup (published / fixed), and 64 groups at 116.36 / 142.57 ns. The latter is approximately +26 ns per lookup, or +1.68 microseconds for a pass over all 64 groups, with wide ShortRun intervals. All scale cases have zero steady-state allocations and monitor contentions. This cost is retained explicitly alongside the Default-job gains for three-group requests and contended lookups; the implementation is not claimed to win at every group count.

## Reproduce

Copy these three benchmark sources and the benchmark project references to each revision, build
Release, and run sequentially from `benchmarks/VirtoCommerce.Platform.Benchmark`:

```console
dotnet run -c Release -- --filter '*CacheMetrics*Benchmarks*' --job dry --exporters json --artifacts ./smoke
dotnet run -c Release -- --filter '*CacheMetrics*Benchmarks*' --exporters json --artifacts ./measured
```

Dry verifies harness execution only. The tables use the default warmed job. A ShortRun probe was used
before the complete matrix; it is not substituted into these tables.

## CacheMetricsBenchmarks — same-host warmed BenchmarkDotNet comparison

Times are ns/operation (batch methods: ns/batch). Each timing cell is Mean ± 99.9% confidence-interval margin (StdDev). Dedicated-worker or scheduler overhead is present on all revisions.

| Method | State | Base ns | Published ns | Fixed ns | Fixed/base | Bytes base / published / fixed | Monitor contentions base / published / fixed |
|---|---|---:|---:|---:|---:|---:|---:|
| TypedKey | Meter | 11.68 ± 0.27 (0.31) | 12.06 ± 0.26 (0.25) | 12.13 ± 0.19 (0.17) | 1.04× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedKeyEightThreads | Meter | 4.33 ± 0.08 (0.07) | 4.71 ± 0.09 (0.14) | 4.56 ± 0.09 (0.14) | 1.05× | 88 / 88 / 88 | 5.07E-007 / 1.27E-006 / 5.81E-007 |
| TypedHit | Meter | 50.59 ± 0.58 (0.52) | 88.34 ± 0.92 (0.86) | 82.19 ± 0.74 (0.69) | 1.62× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| UntypedHit | Meter | 45.55 ± 0.13 (0.12) | 49.41 ± 0.43 (0.40) | 49.80 ± 1.00 (0.83) | 1.09× | 0 / 0 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedHitEightThreads | Meter | 20.80 ± 0.14 (0.12) | 24.38 ± 0.45 (0.42) | 21.90 ± 0.23 (0.19) | 1.05× | 88 / 88 / 88 | 4.77E-007 / 2.09E-007 / 2.68E-007 |
| ExclusiveWarmHit | Meter | 103.86 ± 1.19 (1.05) | 137.47 ± 1.15 (1.07) | 131.48 ± 1.24 (1.16) | 1.27× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| PlatformWarmBatch | Meter | 6487.25 ± 46.51 (43.51) | 8200.43 ± 98.58 (92.21) | 7921.37 ± 110.10 (91.94) | 1.22× | 7192 / 7192 / 7192 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestByKeyHit | Meter | 12.07 ± 0.10 (0.09) | 34.58 ± 0.70 (0.62) | 33.81 ± 0.27 (0.24) | 2.80× | 0 / 0 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestWarmBatch | Meter | 2761.97 ± 17.22 (15.27) | 2537.44 ± 28.48 (26.64) | 2514.00 ± 13.91 (12.33) | 0.91× | 1888 / 1888 / 1888 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CrudConstructor | Meter | 29.28 ± 0.26 (0.20) | 35.44 ± 0.60 (0.54) | 34.97 ± 0.18 (0.16) | 1.19× | 48 / 48 / 48 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedKey | Off | 12.15 ± 0.20 (0.25) | 12.18 ± 0.19 (0.18) | 13.22 ± 0.30 (0.85) | 1.09× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedKeyEightThreads | Off | 4.91 ± 0.09 (0.09) | 4.63 ± 0.09 (0.10) | 5.07 ± 0.10 (0.14) | 1.03× | 88 / 88 / 88 | 1.18E-006 / 1.40E-006 / 1.42E-007 |
| TypedHit | Off | 51.14 ± 0.45 (0.42) | 51.64 ± 0.44 (0.39) | 52.29 ± 0.64 (0.57) | 1.02× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| UntypedHit | Off | 42.07 ± 0.27 (0.22) | 44.02 ± 0.28 (0.25) | 45.29 ± 0.91 (0.89) | 1.08× | 0 / 0 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedHitEightThreads | Off | 20.12 ± 0.13 (0.11) | 20.63 ± 0.27 (0.25) | 14.81 ± 0.12 (0.10) | 0.74× | 88 / 88 / 88 | 1.79E-007 / 3.87E-007 / 2.09E-007 |
| ExclusiveWarmHit | Off | 107.63 ± 0.65 (0.61) | 107.18 ± 0.92 (0.86) | 112.60 ± 1.96 (1.84) | 1.05× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| PlatformWarmBatch | Off | 6807.56 ± 76.37 (71.44) | 6813.44 ± 91.85 (85.92) | 7117.48 ± 96.50 (90.27) | 1.05× | 7192 / 7192 / 7192 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestByKeyHit | Off | 12.03 ± 0.10 (0.08) | 12.26 ± 0.03 (0.02) | 12.94 ± 0.26 (0.52) | 1.08× | 0 / 0 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestWarmBatch | Off | 2795.47 ± 18.87 (16.73) | 2489.77 ± 33.98 (31.78) | 2655.81 ± 51.50 (96.73) | 0.95× | 1888 / 1888 / 1888 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CrudConstructor | Off | 30.50 ± 0.55 (0.54) | 35.70 ± 0.74 (0.88) | 34.70 ± 0.16 (0.13) | 1.14× | 48 / 48 / 48 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedKey | OffAfterRequest | 11.79 ± 0.15 (0.13) | 11.93 ± 0.14 (0.13) | 12.01 ± 0.15 (0.14) | 1.02× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedKeyEightThreads | OffAfterRequest | 4.60 ± 0.09 (0.19) | 4.88 ± 0.10 (0.16) | 4.64 ± 0.09 (0.18) | 1.01× | 88 / 88 / 88 | 1.19E-006 / 1.64E-006 / 9.54E-007 |
| TypedHit | OffAfterRequest | 51.23 ± 0.60 (0.53) | 55.43 ± 1.03 (2.34) | 57.83 ± 0.87 (0.73) | 1.13× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| UntypedHit | OffAfterRequest | 43.18 ± 0.87 (1.00) | 48.10 ± 0.19 (0.15) | 44.96 ± 0.36 (0.32) | 1.04× | 0 / 0 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedHitEightThreads | OffAfterRequest | 20.49 ± 0.24 (0.20) | 24.93 ± 0.49 (0.62) | 23.44 ± 0.46 (0.47) | 1.14× | 88 / 88 / 88 | 4.47E-007 / 2.68E-007 / 1.49E-007 |
| ExclusiveWarmHit | OffAfterRequest | 107.82 ± 1.12 (1.05) | 102.32 ± 0.97 (0.91) | 101.19 ± 0.57 (0.50) | 0.94× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| PlatformWarmBatch | OffAfterRequest | 6636.39 ± 132.34 (129.97) | 6878.05 ± 74.31 (69.51) | 6532.96 ± 129.82 (121.44) | 0.98× | 7192 / 7192 / 7192 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestByKeyHit | OffAfterRequest | 12.10 ± 0.11 (0.10) | 13.69 ± 0.06 (0.06) | 13.59 ± 0.08 (0.08) | 1.12× | 0 / 0 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestWarmBatch | OffAfterRequest | 2763.81 ± 12.71 (11.27) | 2486.72 ± 29.63 (26.27) | 2476.10 ± 24.86 (20.76) | 0.90× | 1888 / 1888 / 1888 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CrudConstructor | OffAfterRequest | 30.21 ± 0.54 (0.51) | 34.95 ± 0.37 (0.35) | 35.20 ± 0.27 (0.21) | 1.17× | 48 / 48 / 48 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedKey | Request | 11.85 ± 0.19 (0.16) | 12.28 ± 0.13 (0.11) | 12.24 ± 0.20 (0.19) | 1.03× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedKeyEightThreads | Request | 4.86 ± 0.07 (0.06) | 4.63 ± 0.05 (0.06) | 4.98 ± 0.10 (0.19) | 1.02× | 88 / 88 / 88 | 8.57E-007 / 1.07E-006 / 1.44E-006 |
| TypedHit | Request | 59.05 ± 1.12 (0.99) | 93.53 ± 0.44 (0.39) | 88.28 ± 1.22 (1.14) | 1.50× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| UntypedHit | Request | 42.04 ± 0.11 (0.10) | 58.78 ± 0.28 (0.26) | 55.01 ± 0.62 (0.58) | 1.31× | 0 / 0 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedHitEightThreads | Request | 20.55 ± 0.09 (0.07) | 54.54 ± 1.04 (1.15) | 37.47 ± 0.70 (0.72) | 1.82× | 88 / 88 / 88 | 3.28E-007 / 2.38E-007 / 1.19E-007 |
| ExclusiveWarmHit | Request | 107.01 ± 0.62 (0.58) | 150.98 ± 2.96 (3.53) | 142.66 ± 1.93 (1.80) | 1.33× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| PlatformWarmBatch | Request | 6773.60 ± 45.28 (40.14) | 9087.99 ± 81.76 (72.48) | 8139.89 ± 38.16 (33.83) | 1.20× | 7192 / 7192 / 7192 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestByKeyHit | Request | 12.28 ± 0.23 (0.19) | 47.51 ± 0.45 (0.40) | 38.73 ± 0.33 (0.29) | 3.15× | 0 / 0 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestWarmBatch | Request | 2799.70 ± 22.77 (20.19) | 2501.36 ± 21.57 (18.01) | 2678.24 ± 13.67 (12.11) | 0.96× | 1888 / 1888 / 1888 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CrudConstructor | Request | 30.14 ± 0.45 (0.42) | 35.05 ± 0.32 (0.26) | 35.14 ± 0.24 (0.20) | 1.17× | 48 / 48 / 48 | 0.00E+000 / 0.00E+000 / 0.00E+000 |

Gen0 collections per 1,000 operations:

| Method | State | Base | Published | Fixed |
|---|---|---:|---:|---:|
| TypedKey | Meter | 0.0105 | 0.0105 | 0.0105 |
| TypedKeyEightThreads | Meter | 0.0106 | 0.0106 | 0.0106 |
| TypedHit | Meter | 0.0105 | 0.0105 | 0.0105 |
| UntypedHit | Meter | 0.0000 | 0.0000 | 0.0000 |
| TypedHitEightThreads | Meter | 0.0106 | 0.0106 | 0.0106 |
| ExclusiveWarmHit | Meter | 0.0105 | 0.0105 | 0.0105 |
| PlatformWarmBatch | Meter | 0.8545 | 0.8545 | 0.8545 |
| RequestByKeyHit | Meter | 0.0000 | 0.0000 | 0.0000 |
| RequestWarmBatch | Meter | 0.2251 | 0.2251 | 0.2251 |
| CrudConstructor | Meter | 0.0057 | 0.0057 | 0.0057 |
| TypedKey | Off | 0.0105 | 0.0105 | 0.0105 |
| TypedKeyEightThreads | Off | 0.0106 | 0.0106 | 0.0106 |
| TypedHit | Off | 0.0105 | 0.0105 | 0.0105 |
| UntypedHit | Off | 0.0000 | 0.0000 | 0.0000 |
| TypedHitEightThreads | Off | 0.0106 | 0.0106 | 0.0106 |
| ExclusiveWarmHit | Off | 0.0105 | 0.0105 | 0.0105 |
| PlatformWarmBatch | Off | 0.8545 | 0.8545 | 0.8545 |
| RequestByKeyHit | Off | 0.0000 | 0.0000 | 0.0000 |
| RequestWarmBatch | Off | 0.2251 | 0.2251 | 0.2251 |
| CrudConstructor | Off | 0.0057 | 0.0057 | 0.0057 |
| TypedKey | OffAfterRequest | 0.0105 | 0.0105 | 0.0105 |
| TypedKeyEightThreads | OffAfterRequest | 0.0106 | 0.0106 | 0.0106 |
| TypedHit | OffAfterRequest | 0.0105 | 0.0105 | 0.0105 |
| UntypedHit | OffAfterRequest | 0.0000 | 0.0000 | 0.0000 |
| TypedHitEightThreads | OffAfterRequest | 0.0106 | 0.0106 | 0.0106 |
| ExclusiveWarmHit | OffAfterRequest | 0.0105 | 0.0105 | 0.0105 |
| PlatformWarmBatch | OffAfterRequest | 0.8545 | 0.8545 | 0.8545 |
| RequestByKeyHit | OffAfterRequest | 0.0000 | 0.0000 | 0.0000 |
| RequestWarmBatch | OffAfterRequest | 0.2251 | 0.2251 | 0.2251 |
| CrudConstructor | OffAfterRequest | 0.0057 | 0.0057 | 0.0057 |
| TypedKey | Request | 0.0105 | 0.0105 | 0.0105 |
| TypedKeyEightThreads | Request | 0.0106 | 0.0106 | 0.0106 |
| TypedHit | Request | 0.0105 | 0.0105 | 0.0105 |
| UntypedHit | Request | 0.0000 | 0.0000 | 0.0000 |
| TypedHitEightThreads | Request | 0.0106 | 0.0106 | 0.0106 |
| ExclusiveWarmHit | Request | 0.0105 | 0.0105 | 0.0105 |
| PlatformWarmBatch | Request | 0.8545 | 0.8545 | 0.8545 |
| RequestByKeyHit | Request | 0.0000 | 0.0000 | 0.0000 |
| RequestWarmBatch | Request | 0.2251 | 0.2251 | 0.2251 |
| CrudConstructor | Request | 0.0057 | 0.0057 | 0.0057 |


## CacheMetricsContentionBenchmarks — same-host warmed BenchmarkDotNet comparison

Times are ns/operation (batch methods: ns/batch). Each timing cell is Mean ± 99.9% confidence-interval margin (StdDev). Dedicated-worker or scheduler overhead is present on all revisions.

| Method | State | Base ns | Published ns | Fixed ns | Fixed/base | Bytes base / published / fixed | Monitor contentions base / published / fixed |
|---|---|---:|---:|---:|---:|---:|---:|
| TypedKey | Meter | 4.76 ± 0.09 (0.26) | 4.84 ± 0.09 (0.23) | 5.03 ± 0.10 (0.27) | 1.06× | 88 / 88 / 88 | 2.96E-004 / 2.80E-004 / 3.03E-004 |
| TypedHitInOneRequest | Meter | 20.87 ± 0.11 (0.09) | 22.39 ± 0.37 (0.35) | 22.81 ± 0.45 (0.44) | 1.09× | 88 / 88 / 88 | 2.14E-004 / 2.56E-004 / 2.24E-004 |
| TypedKey | Off | 4.90 ± 0.12 (0.35) | 4.69 ± 0.09 (0.24) | 5.12 ± 0.15 (0.42) | 1.05× | 88 / 88 / 88 | 2.79E-004 / 2.75E-004 / 2.76E-004 |
| TypedHitInOneRequest | Off | 20.24 ± 0.23 (0.21) | 20.55 ± 0.22 (0.19) | 20.33 ± 0.40 (0.43) | 1.00× | 88 / 88 / 88 | 1.67E-004 / 1.79E-004 / 2.36E-004 |
| TypedKey | OffAfterRequest | 4.79 ± 0.11 (0.31) | 4.90 ± 0.10 (0.11) | 5.06 ± 0.11 (0.33) | 1.06× | 88 / 88 / 88 | 2.56E-004 / 2.82E-004 / 2.76E-004 |
| TypedHitInOneRequest | OffAfterRequest | 21.04 ± 0.10 (0.09) | 20.89 ± 0.09 (0.08) | 20.54 ± 0.40 (0.44) | 0.98× | 88 / 88 / 88 | 2.05E-004 / 1.93E-004 / 1.86E-004 |
| TypedKey | Request | 4.88 ± 0.10 (0.26) | 5.36 ± 0.23 (0.67) | 5.14 ± 0.12 (0.35) | 1.05× | 88 / 88 / 88 | 2.76E-004 / 2.71E-004 / 2.85E-004 |
| TypedHitInOneRequest | Request | 20.36 ± 0.18 (0.16) | 62.54 ± 0.71 (0.66) | 23.17 ± 0.45 (0.42) | 1.14× | 88 / 88 / 88 | 1.23E-004 / 2.57E-004 / 2.43E-004 |

Gen0 collections per 1,000 operations:

| Method | State | Base | Published | Fixed |
|---|---|---:|---:|---:|
| TypedKey | Meter | 0.0106 | 0.0106 | 0.0106 |
| TypedHitInOneRequest | Meter | 0.0105 | 0.0106 | 0.0106 |
| TypedKey | Off | 0.0106 | 0.0106 | 0.0106 |
| TypedHitInOneRequest | Off | 0.0106 | 0.0106 | 0.0106 |
| TypedKey | OffAfterRequest | 0.0106 | 0.0106 | 0.0106 |
| TypedHitInOneRequest | OffAfterRequest | 0.0106 | 0.0106 | 0.0106 |
| TypedKey | Request | 0.0106 | 0.0106 | 0.0106 |
| TypedHitInOneRequest | Request | 0.0106 | 0.0105 | 0.0106 |


## CacheMetricsRequestBenchmarks — same-host warmed BenchmarkDotNet comparison

Times are ns/operation (batch methods: ns/batch). Each timing cell is Mean ± 99.9% confidence-interval margin (StdDev). Dedicated-worker or scheduler overhead is present on all revisions.

| Method | State | Base ns | Published ns | Fixed ns | Fixed/base | Bytes base / published / fixed | Monitor contentions base / published / fixed |
|---|---|---:|---:|---:|---:|---:|---:|
| CompleteRequest | Off&Groups=0&AsyncDownstream=False | 227.01 ± 4.56 (5.25) | 264.90 ± 6.27 (17.39) | 277.56 ± 5.51 (9.20) | 1.22× | 1464 / 1464 / 1464 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | Off&Groups=0&AsyncDownstream=True | 240.99 ± 4.75 (10.72) | 257.14 ± 4.89 (7.61) | 254.47 ± 5.10 (11.72) | 1.06× | 1552 / 1552 / 1552 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | Off&Groups=3&AsyncDownstream=False | 368.69 ± 7.31 (9.51) | 394.33 ± 7.91 (14.26) | 385.67 ± 6.56 (6.14) | 1.05× | 1608 / 1608 / 1608 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | Off&Groups=3&AsyncDownstream=True | 379.45 ± 6.24 (5.21) | 401.98 ± 5.64 (4.71) | 401.12 ± 7.98 (19.28) | 1.06× | 1696 / 1696 / 1696 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | OffAfterRequest&Groups=0&AsyncDownstream=False | 224.25 ± 4.51 (4.43) | 242.85 ± 4.87 (6.98) | 242.76 ± 4.38 (3.89) | 1.08× | 1464 / 1464 / 1464 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | OffAfterRequest&Groups=0&AsyncDownstream=True | 231.02 ± 4.60 (7.02) | 247.46 ± 4.79 (7.17) | 252.49 ± 4.76 (8.20) | 1.09× | 1552 / 1552 / 1552 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | OffAfterRequest&Groups=3&AsyncDownstream=False | 359.75 ± 6.37 (5.95) | 390.05 ± 6.63 (6.20) | 390.95 ± 7.63 (7.84) | 1.09× | 1608 / 1608 / 1608 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | OffAfterRequest&Groups=3&AsyncDownstream=True | 380.53 ± 7.62 (13.14) | 408.03 ± 7.94 (12.36) | 402.96 ± 7.93 (12.58) | 1.06× | 1696 / 1696 / 1696 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | Recorded&Groups=0&AsyncDownstream=False | 222.52 ± 3.84 (3.41) | 281.26 ± 5.14 (4.81) | 276.14 ± 4.50 (4.42) | 1.24× | 1464 / 1640 / 1608 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | Recorded&Groups=0&AsyncDownstream=True | 242.92 ± 4.80 (4.25) | 344.84 ± 5.99 (5.31) | 339.67 ± 6.40 (13.07) | 1.40× | 1552 / 1856 / 1824 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | Recorded&Groups=3&AsyncDownstream=False | 380.74 ± 7.48 (7.00) | 1042.62 ± 19.74 (20.27) | 676.23 ± 13.02 (11.54) | 1.78× | 1608 / 3280 / 2232 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | Recorded&Groups=3&AsyncDownstream=True | 383.76 ± 4.24 (3.31) | 1152.07 ± 20.68 (18.34) | 756.15 ± 14.75 (22.08) | 1.97× | 1696 / 3496 / 2448 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | RecordOnly&Groups=0&AsyncDownstream=False | 226.74 ± 3.66 (9.76) | 246.84 ± 4.71 (3.93) | 270.19 ± 4.73 (3.95) | 1.19× | 1464 / 1464 / 1464 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | RecordOnly&Groups=0&AsyncDownstream=True | 239.25 ± 4.71 (8.49) | 266.14 ± 5.22 (9.01) | 263.37 ± 3.39 (2.83) | 1.10× | 1552 / 1552 / 1552 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | RecordOnly&Groups=3&AsyncDownstream=False | 370.15 ± 7.20 (9.11) | 439.69 ± 2.94 (2.60) | 445.09 ± 8.31 (16.41) | 1.20× | 1608 / 1608 / 1608 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | RecordOnly&Groups=3&AsyncDownstream=True | 393.92 ± 5.55 (4.64) | 454.07 ± 8.86 (8.29) | 472.10 ± 8.65 (9.26) | 1.20× | 1696 / 1696 / 1696 | 0.00E+000 / 0.00E+000 / 0.00E+000 |

Gen0 collections per 1,000 operations:

| Method | State | Base | Published | Fixed |
|---|---|---:|---:|---:|
| CompleteRequest | Off&Groups=0&AsyncDownstream=False | 0.1750 | 0.1750 | 0.1750 |
| CompleteRequest | Off&Groups=0&AsyncDownstream=True | 0.1855 | 0.1855 | 0.1855 |
| CompleteRequest | Off&Groups=3&AsyncDownstream=False | 0.1922 | 0.1922 | 0.1922 |
| CompleteRequest | Off&Groups=3&AsyncDownstream=True | 0.2027 | 0.2027 | 0.2027 |
| CompleteRequest | OffAfterRequest&Groups=0&AsyncDownstream=False | 0.1750 | 0.1750 | 0.1750 |
| CompleteRequest | OffAfterRequest&Groups=0&AsyncDownstream=True | 0.1855 | 0.1855 | 0.1855 |
| CompleteRequest | OffAfterRequest&Groups=3&AsyncDownstream=False | 0.1922 | 0.1922 | 0.1922 |
| CompleteRequest | OffAfterRequest&Groups=3&AsyncDownstream=True | 0.2027 | 0.2027 | 0.2027 |
| CompleteRequest | Recorded&Groups=0&AsyncDownstream=False | 0.1750 | 0.1960 | 0.1922 |
| CompleteRequest | Recorded&Groups=0&AsyncDownstream=True | 0.1855 | 0.2217 | 0.2179 |
| CompleteRequest | Recorded&Groups=3&AsyncDownstream=False | 0.1922 | 0.3910 | 0.2661 |
| CompleteRequest | Recorded&Groups=3&AsyncDownstream=True | 0.2027 | 0.4177 | 0.2918 |
| CompleteRequest | RecordOnly&Groups=0&AsyncDownstream=False | 0.1750 | 0.1750 | 0.1750 |
| CompleteRequest | RecordOnly&Groups=0&AsyncDownstream=True | 0.1855 | 0.1855 | 0.1855 |
| CompleteRequest | RecordOnly&Groups=3&AsyncDownstream=False | 0.1922 | 0.1922 | 0.1922 |
| CompleteRequest | RecordOnly&Groups=3&AsyncDownstream=True | 0.2027 | 0.2027 | 0.2027 |

## Group-count scaling diagnostic (ShortRun)

The three additional cases per revision use one sampled scope, pre-normalized warm keys and a no-op meter listener. Each invocation visits every group once. The table divides batch Mean by Groups for ns/lookup; full batch confidence intervals and diagnostics follow. This is a ShortRun diagnostic with only three measurement iterations, not part of the 192 Default cases. Wide intervals limit timing precision.

| Groups | Base ns/lookup | Published ns/lookup | Fixed ns/lookup | Bytes per pass base / published / fixed |
|---:|---:|---:|---:|---:|
| 1 | 55.37 | 83.73 | 80.72 | 0 / 0 / 0 |
| 16 | 74.90 | 113.51 | 115.95 | 0 / 0 / 0 |
| 64 | 73.53 | 116.36 | 142.57 | 0 / 0 / 0 |

Additional identical source: `CacheGroupScaleBenchmarks.cs`, normalized SHA-256 `19DFF14742B09F05EB78F076C553A02CCEE64DD6BD6A6544EF213626200659B1`. Dry3 passed on the fixed implementation before measurement.

```console
dotnet run -c Release -- --filter '*CacheGroupScaleBenchmarks*' --job short --exporters json --artifacts ./scale
```

### Full ShortRun diagnostics

Times are ns/operation (batch methods: ns/batch). Each timing cell is Mean ± 99.9% confidence-interval margin (StdDev). Dedicated-worker or scheduler overhead is present on all revisions.

| Method | State | Base ns | Published ns | Fixed ns | Fixed/base | Bytes base / published / fixed | Monitor contentions base / published / fixed |
|---|---|---:|---:|---:|---:|---:|---:|
| ReadAllGroups | Groups=1 | 55.37 ± 2.64 (0.14) | 83.73 ± 1.87 (0.10) | 80.72 ± 34.29 (1.88) | 1.46× | 0 / 0 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| ReadAllGroups | Groups=16 | 1198.41 ± 18.25 (1.00) | 1816.21 ± 163.55 (8.96) | 1855.26 ± 101.23 (5.55) | 1.55× | 0 / 0 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| ReadAllGroups | Groups=64 | 4705.71 ± 208.07 (11.40) | 7447.07 ± 354.07 (19.41) | 9124.34 ± 1581.53 (86.69) | 1.94× | 0 / 0 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |

Gen0 collections per 1,000 operations:

| Method | State | Base | Published | Fixed |
|---|---|---:|---:|---:|
| ReadAllGroups | Groups=1 | 0.0000 | 0.0000 | 0.0000 |
| ReadAllGroups | Groups=16 | 0.0000 | 0.0000 | 0.0000 |
| ReadAllGroups | Groups=64 | 0.0000 | 0.0000 | 0.0000 |

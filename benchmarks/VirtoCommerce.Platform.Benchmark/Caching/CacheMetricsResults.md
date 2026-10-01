# Cache telemetry benchmark comparison — 2026-10-01

Same-host comparison for the cache instrumentation changes in PR #3123:

- **Base:** `658d74c4247724cb0d260360fc159ef1e67a40d3` (before instrumentation).
- **Reviewed:** `d0ba70fad231f0ed33b56840ad5c330e2c7a9b2b`.
- **Fixed:** the implementation and benchmark sources accompanying this report.
- Windows 11 25H2, AMD Ryzen 7 8845HS (8 cores / 16 logical processors), SDK 10.0.400,
  runtime 10.0.11, BenchmarkDotNet 0.15.8, Release, default warmed job.

## Method

The same three benchmark files and project references were compiled against all three revisions.
Reflection only adapts setup to the pre-instrumentation baseline. Runs were sequential, without
concurrent builds or tests. The full matrix contains 40 cases per revision: 30 warm-operation,
6 fixed-thread contention, and 4 complete-request lifecycle cases.

`Off` has no listener or accumulator. `Meter` uses a no-op listener. `Request` adds a sampled
accumulator where available; the baseline has no accumulator. These microbenchmarks isolate the
platform's instrumentation, not an OpenTelemetry exporter's aggregation, network cost or application
throughput. The warm suites exclude initialization/completion; the lifecycle suite includes them.

Times are per operation; the two batch methods are per 50-ID batch. The main parallel cases use eight
`Parallel.For` partitions (not guaranteed eight active threads). The contention suite uses eight
fixed threads and amortizes its barriers over 32,768 lookups. Their small nonzero monitor counts
include the harness barriers; they do not by themselves identify a cache monitor.

The 88 B in typed-key creation and typed memory hits also exist in the baseline: the key string and
case normalization still allocate. The removed telemetry prefix allocation is additional to that
baseline. Absolute values from another host or owner-prefix length are not directly comparable.
Confidence intervals describe each run; small differences between processes also reflect normal
JIT/scheduling/clock variation. These are measured costs, not production throughput guarantees.

Normalized SHA-256 (LF and one final newline), identical across the three checkouts:

- `CacheMetricsBenchmarks.cs`: `FD363C7E3F3B4A5C747C07DC5C3E30C87C27F47BE88B7B4FBD5A5D5412A004FF`
- `CacheMetricsContentionBenchmarks.cs`: `FF0EA267883948F76A021E32E078EE15AF072EDF62613890C0696092E0F8A51C`
- `CacheMetricsRequestBenchmarks.cs`: `64D5E4EE5ED9BDF93F65C2DF62FC32B851050D72DF6300C3BBE8DCA0CA7F07E2`

## Findings

- All ten `Off` warm-operation cases have the same allocated bytes as the baseline. Single-threaded
  cases have zero monitor contentions. Parallel diagnostics include harness scheduling/barriers;
  the steady key/registration paths contain no telemetry monitor or dictionary write.
- Off typed-key creation is **12.43 → 38.58 → 12.34 ns** (base → reviewed → fixed). On eight fixed
  threads it is **5.12 → 83.45 → 4.86 ns/op**. Off request by-key is **12.14 → 44.69 → 13.51 ns**,
  with **0 → 72 → 0 B**. A warm 50-ID request batch is **2807.87 → 4052.99 → 2494.51 ns**,
  with **1888 → 5488 → 1888 B**.
- Typed memory hits in Meter/Request return from 160 B to the baseline 88 B. With eight fixed
  threads in one sampled request, **141.52 → 64.61 ns/op** replaces the contended reviewed path;
  the baseline without request attribution is 20.41 ns/op. Snapshot synchronization still has a cost.
- Remaining costs are explicit: Off CRUD construction is **35.25 vs 30.18 ns** in base; Off request
  by-key is **13.51 vs 12.14 ns**. A single Meter typed hit is **83.12 vs 79.94 ns** in the reviewed
  version, despite removing its extra allocation. This is not an across-the-board timing improvement.
- Complete RecordOnly requests have baseline allocations again: **1464 B** with zero groups and
  **1608 B** with three. Three-group time falls from **880.76 to 424.15 ns** (baseline 347.98 ns).
  Empty sampled requests allocate **1640 B**, versus 1968 B reviewed and 1464 B baseline; creating
  the sampled scope still costs 176 B even though no cache tags are published.
- Complete sampled requests with three groups cost **1018.87 ns / 3280 B**, versus
  **902.98 ns / 3784 B** reviewed and **351.16 ns / 1608 B** baseline. That is **115.89 ns more CPU
  and 504 B less allocation than reviewed** in this case. The final implementation serializes one
  bounded JSON summary and publishes group-outcome metrics instead of creating per-group events.
  SDK3 capture confirms zero summary MessageData rows; this benchmark does not measure the downstream
  exporter/network savings, so no net end-to-end speedup is claimed for this lifecycle case.

## Reproduce

Use the same `Caching/CacheMetrics*Benchmarks.cs` sources and the Caching/Data project references
in the baseline checkout and the branch checkout. From the benchmark project directory:

```console
dotnet run -c Release -- --filter '*CacheMetrics*Benchmarks*' --job dry --exporters json --artifacts ./smoke
dotnet run -c Release -- --filter '*CacheMetrics*Benchmarks*' --exporters json --artifacts ./measured
```

Dry only checks harness execution. All numbers below come from warmed default jobs.

## CacheMetricsBenchmarks — same-host warmed BenchmarkDotNet comparison

Times are ns/operation (batch methods: ns/batch). Each timing cell is Mean ± 99.9% confidence-interval margin (StdDev). Dedicated-worker or scheduler overhead is present on all revisions.

| Method | State | Base ns | Reviewed ns | Fixed ns | Fixed/base | Bytes base / reviewed / fixed | Monitor contentions base / reviewed / fixed |
|---|---|---:|---:|---:|---:|---:|---:|
| TypedKey | Meter | 12.00 ± 0.28 (0.38) | 41.63 ± 0.54 (0.48) | 12.56 ± 0.28 (0.30) | 1.05× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedKeyEightThreads | Meter | 5.32 ± 0.21 (0.55) | 86.22 ± 1.67 (2.56) | 4.69 ± 0.09 (0.16) | 0.88× | 88 / 88 / 88 | 7.82E-007 / 8.23E-005 / 9.02E-007 |
| TypedHit | Meter | 51.49 ± 0.60 (0.73) | 79.94 ± 0.72 (0.64) | 83.12 ± 1.33 (1.24) | 1.61× | 88 / 160 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| UntypedHit | Meter | 42.27 ± 0.40 (0.33) | 48.62 ± 0.46 (0.43) | 49.26 ± 0.35 (0.33) | 1.17× | 0 / 0 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedHitEightThreads | Meter | 20.34 ± 0.23 (0.19) | 37.06 ± 0.74 (1.97) | 21.80 ± 0.20 (0.16) | 1.07× | 88 / 160 / 88 | 2.09E-007 / 4.17E-007 / 1.49E-007 |
| ExclusiveWarmHit | Meter | 108.03 ± 1.13 (0.88) | 150.77 ± 1.69 (1.58) | 136.03 ± 1.54 (1.44) | 1.26× | 88 / 160 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| PlatformWarmBatch | Meter | 6528.93 ± 53.00 (49.57) | 8500.21 ± 96.28 (85.35) | 8360.78 ± 81.41 (72.16) | 1.28× | 7192 / 10792 / 7192 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestByKeyHit | Meter | 12.20 ± 0.09 (0.07) | 43.34 ± 0.50 (0.41) | 33.97 ± 0.29 (0.24) | 2.78× | 0 / 72 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestWarmBatch | Meter | 2769.05 ± 22.05 (18.41) | 4277.69 ± 77.72 (129.86) | 2502.09 ± 22.68 (20.11) | 0.90× | 1888 / 5488 / 1888 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CrudConstructor | Meter | 30.98 ± 0.29 (0.24) | 60.78 ± 0.79 (0.74) | 35.21 ± 0.31 (0.26) | 1.14× | 48 / 48 / 48 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedKey | Off | 12.43 ± 0.27 (0.50) | 38.58 ± 0.60 (0.56) | 12.34 ± 0.23 (0.21) | 0.99× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedKeyEightThreads | Off | 4.68 ± 0.09 (0.19) | 89.79 ± 1.76 (3.09) | 4.69 ± 0.09 (0.10) | 1.00× | 88 / 88 / 88 | 9.76E-007 / 1.01E-004 / 1.26E-006 |
| TypedHit | Off | 52.15 ± 0.77 (0.68) | 53.67 ± 0.75 (0.67) | 51.25 ± 0.28 (0.25) | 0.98× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| UntypedHit | Off | 51.61 ± 1.03 (1.27) | 51.12 ± 0.25 (0.24) | 44.63 ± 0.65 (0.58) | 0.86× | 0 / 0 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedHitEightThreads | Off | 20.16 ± 0.20 (0.19) | 25.31 ± 0.50 (1.24) | 20.46 ± 0.18 (0.14) | 1.01× | 88 / 88 / 88 | 6.56E-007 / 2.38E-007 / 4.17E-007 |
| ExclusiveWarmHit | Off | 108.09 ± 1.57 (1.47) | 108.77 ± 0.96 (0.90) | 101.07 ± 0.56 (0.49) | 0.94× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| PlatformWarmBatch | Off | 6897.65 ± 51.45 (48.13) | 6985.41 ± 109.02 (101.98) | 6837.74 ± 62.07 (55.03) | 0.99× | 7192 / 7192 / 7192 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestByKeyHit | Off | 12.14 ± 0.14 (0.11) | 44.69 ± 0.64 (0.60) | 13.51 ± 0.12 (0.11) | 1.11× | 0 / 72 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestWarmBatch | Off | 2807.87 ± 39.75 (35.23) | 4052.99 ± 54.91 (48.67) | 2494.51 ± 26.17 (23.20) | 0.89× | 1888 / 5488 / 1888 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CrudConstructor | Off | 30.18 ± 0.32 (0.28) | 61.33 ± 0.54 (0.48) | 35.25 ± 0.37 (0.35) | 1.17× | 48 / 48 / 48 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedKey | Request | 12.19 ± 0.24 (0.22) | 38.75 ± 0.67 (0.63) | 12.30 ± 0.16 (0.13) | 1.01× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedKeyEightThreads | Request | 5.10 ± 0.10 (0.13) | 89.60 ± 1.18 (1.11) | 4.70 ± 0.09 (0.11) | 0.92× | 88 / 88 / 88 | 1.20E-006 / 8.82E-005 / 1.18E-006 |
| TypedHit | Request | 53.04 ± 0.37 (0.31) | 101.55 ± 0.95 (0.89) | 93.73 ± 0.58 (0.52) | 1.77× | 88 / 160 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| UntypedHit | Request | 42.66 ± 0.39 (0.34) | 69.24 ± 0.59 (0.52) | 58.80 ± 0.41 (0.38) | 1.38× | 0 / 0 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedHitEightThreads | Request | 20.25 ± 0.21 (0.17) | 138.05 ± 0.84 (0.66) | 63.72 ± 0.33 (0.29) | 3.15× | 88 / 160 / 88 | 8.94E-008 / 4.28E-004 / 7.15E-007 |
| ExclusiveWarmHit | Request | 107.76 ± 0.69 (0.61) | 173.76 ± 5.57 (15.72) | 148.67 ± 1.16 (1.03) | 1.38× | 88 / 160 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| PlatformWarmBatch | Request | 6586.27 ± 96.95 (90.69) | 9670.05 ± 218.19 (597.29) | 8938.33 ± 89.91 (79.70) | 1.36× | 7192 / 10792 / 7192 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestByKeyHit | Request | 12.29 ± 0.18 (0.23) | 61.11 ± 1.23 (1.15) | 47.37 ± 0.22 (0.19) | 3.85× | 0 / 72 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestWarmBatch | Request | 2846.39 ± 38.32 (35.85) | 5273.71 ± 51.48 (42.99) | 2544.61 ± 17.16 (15.21) | 0.89× | 1888 / 5488 / 1888 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CrudConstructor | Request | 30.13 ± 0.41 (0.39) | 61.88 ± 1.01 (0.84) | 35.38 ± 0.30 (0.27) | 1.17× | 48 / 48 / 48 | 0.00E+000 / 0.00E+000 / 0.00E+000 |

Gen0 collections per 1,000 operations:

| Method | State | Base | Reviewed | Fixed |
|---|---|---:|---:|---:|
| TypedKey | Meter | 0.0105 | 0.0105 | 0.0105 |
| TypedKeyEightThreads | Meter | 0.0106 | 0.0106 | 0.0106 |
| TypedHit | Meter | 0.0105 | 0.0191 | 0.0105 |
| UntypedHit | Meter | 0.0000 | 0.0000 | 0.0000 |
| TypedHitEightThreads | Meter | 0.0106 | 0.0193 | 0.0106 |
| ExclusiveWarmHit | Meter | 0.0105 | 0.0191 | 0.0105 |
| PlatformWarmBatch | Meter | 0.8545 | 1.2817 | 0.8545 |
| RequestByKeyHit | Meter | 0.0000 | 0.0086 | 0.0000 |
| RequestWarmBatch | Meter | 0.2251 | 0.6561 | 0.2251 |
| CrudConstructor | Meter | 0.0057 | 0.0057 | 0.0057 |
| TypedKey | Off | 0.0105 | 0.0105 | 0.0105 |
| TypedKeyEightThreads | Off | 0.0106 | 0.0106 | 0.0106 |
| TypedHit | Off | 0.0105 | 0.0105 | 0.0105 |
| UntypedHit | Off | 0.0000 | 0.0000 | 0.0000 |
| TypedHitEightThreads | Off | 0.0106 | 0.0106 | 0.0106 |
| ExclusiveWarmHit | Off | 0.0105 | 0.0105 | 0.0105 |
| PlatformWarmBatch | Off | 0.8545 | 0.8545 | 0.8545 |
| RequestByKeyHit | Off | 0.0000 | 0.0086 | 0.0000 |
| RequestWarmBatch | Off | 0.2251 | 0.6561 | 0.2251 |
| CrudConstructor | Off | 0.0057 | 0.0057 | 0.0057 |
| TypedKey | Request | 0.0105 | 0.0105 | 0.0105 |
| TypedKeyEightThreads | Request | 0.0106 | 0.0106 | 0.0106 |
| TypedHit | Request | 0.0105 | 0.0191 | 0.0105 |
| UntypedHit | Request | 0.0000 | 0.0000 | 0.0000 |
| TypedHitEightThreads | Request | 0.0106 | 0.0193 | 0.0106 |
| ExclusiveWarmHit | Request | 0.0105 | 0.0191 | 0.0105 |
| PlatformWarmBatch | Request | 0.8545 | 1.2817 | 0.8545 |
| RequestByKeyHit | Request | 0.0000 | 0.0086 | 0.0000 |
| RequestWarmBatch | Request | 0.2251 | 0.6561 | 0.2251 |
| CrudConstructor | Request | 0.0057 | 0.0057 | 0.0057 |


## CacheMetricsContentionBenchmarks — same-host warmed BenchmarkDotNet comparison

Times are ns/operation (batch methods: ns/batch). Each timing cell is Mean ± 99.9% confidence-interval margin (StdDev). Dedicated-worker or scheduler overhead is present on all revisions.

| Method | State | Base ns | Reviewed ns | Fixed ns | Fixed/base | Bytes base / reviewed / fixed | Monitor contentions base / reviewed / fixed |
|---|---|---:|---:|---:|---:|---:|---:|
| TypedKey | Meter | 4.90 ± 0.09 (0.17) | 81.65 ± 1.59 (1.49) | 4.66 ± 0.09 (0.22) | 0.95× | 88 / 88 / 88 | 2.95E-004 / 3.52E-004 / 2.73E-004 |
| TypedHitInOneRequest | Meter | 21.05 ± 0.12 (0.11) | 24.69 ± 0.49 (0.56) | 22.50 ± 0.30 (0.26) | 1.07× | 88 / 160 / 88 | 1.98E-004 / 2.09E-004 / 2.31E-004 |
| TypedKey | Off | 5.12 ± 0.15 (0.45) | 83.45 ± 1.02 (0.85) | 4.86 ± 0.10 (0.21) | 0.95× | 88 / 88 / 88 | 2.66E-004 / 3.92E-004 / 3.00E-004 |
| TypedHitInOneRequest | Off | 20.24 ± 0.31 (0.28) | 21.56 ± 0.27 (0.24) | 20.80 ± 0.10 (0.09) | 1.03× | 88 / 88 / 88 | 1.45E-004 / 1.88E-004 / 2.03E-004 |
| TypedKey | Request | 4.92 ± 0.10 (0.27) | 88.56 ± 0.60 (0.50) | 4.96 ± 0.10 (0.21) | 1.01× | 88 / 88 / 88 | 2.75E-004 / 3.95E-004 / 2.91E-004 |
| TypedHitInOneRequest | Request | 20.41 ± 0.18 (0.16) | 141.52 ± 1.58 (1.47) | 64.61 ± 0.27 (0.25) | 3.17× | 88 / 160 / 88 | 1.59E-004 / 6.67E-004 / 2.47E-004 |

Gen0 collections per 1,000 operations:

| Method | State | Base | Reviewed | Fixed |
|---|---|---:|---:|---:|
| TypedKey | Meter | 0.0106 | 0.0105 | 0.0106 |
| TypedHitInOneRequest | Meter | 0.0106 | 0.0192 | 0.0106 |
| TypedKey | Off | 0.0106 | 0.0105 | 0.0106 |
| TypedHitInOneRequest | Off | 0.0106 | 0.0106 | 0.0106 |
| TypedKey | Request | 0.0106 | 0.0105 | 0.0106 |
| TypedHitInOneRequest | Request | 0.0106 | 0.0191 | 0.0105 |


## CacheMetricsRequestBenchmarks — same-host warmed BenchmarkDotNet comparison

Times are ns/operation (batch methods: ns/batch). Each timing cell is Mean ± 99.9% confidence-interval margin (StdDev). Dedicated-worker or scheduler overhead is present on all revisions.

| Method | State | Base ns | Reviewed ns | Fixed ns | Fixed/base | Bytes base / reviewed / fixed | Monitor contentions base / reviewed / fixed |
|---|---|---:|---:|---:|---:|---:|---:|
| CompleteRequest | Recorded&Groups=0 | 202.08 ± 3.60 (3.37) | 319.73 ± 5.72 (4.78) | 262.17 ± 5.16 (9.94) | 1.30× | 1464 / 1968 / 1640 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | Recorded&Groups=3 | 351.16 ± 6.51 (6.09) | 902.98 ± 17.86 (17.54) | 1018.87 ± 17.45 (15.47) | 2.90× | 1608 / 3784 / 3280 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | RecordOnly&Groups=0 | 198.39 ± 2.16 (1.92) | 319.06 ± 6.15 (11.84) | 241.71 ± 4.69 (4.16) | 1.22× | 1464 / 1968 / 1464 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | RecordOnly&Groups=3 | 347.98 ± 6.34 (5.62) | 880.76 ± 16.10 (22.57) | 424.15 ± 5.72 (5.35) | 1.22× | 1608 / 3784 / 1608 | 0.00E+000 / 0.00E+000 / 0.00E+000 |

Gen0 collections per 1,000 operations:

| Method | State | Base | Reviewed | Fixed |
|---|---|---:|---:|---:|
| CompleteRequest | Recorded&Groups=0 | 0.1750 | 0.2351 | 0.1960 |
| CompleteRequest | Recorded&Groups=3 | 0.1922 | 0.4520 | 0.3910 |
| CompleteRequest | RecordOnly&Groups=0 | 0.1750 | 0.2351 | 0.1750 |
| CompleteRequest | RecordOnly&Groups=3 | 0.1922 | 0.4520 | 0.1922 |

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

- All ten `Off` warm-operation cases have the same allocated bytes as the baseline. Single-threaded cases have zero monitor contentions. Parallel diagnostics include harness scheduling/barriers; steady key/registration paths contain no telemetry monitor or dictionary write.
- Off typed-key creation is **12.43 → 38.58 → 12.54 ns** (base → reviewed → fixed). On eight fixed threads it is **5.12 → 83.45 → 4.83 ns/op**. Off request by-key is **12.14 → 44.69 → 17.30 ns**, with **0 → 72 → 0 B**. A warm 50-ID request batch is **2807.87 → 4052.99 → 2458.52 ns**, with **1888 → 5488 → 1888 B**.
- Typed memory hits in Meter/Request return from 160 B to the baseline 88 B. With eight fixed threads in one sampled request, **141.52 → 63.14 ns/op** replaces the contended reviewed path; baseline without request attribution is 20.41 ns/op. Snapshot synchronization still has a cost.
- Remaining costs are explicit: Off CRUD construction is **35.95 vs 30.18 ns** in base; Off request by-key is **17.30 vs 12.14 ns**. A single Meter typed hit is **82.52 ns**, compared with 79.94 ns reviewed and 51.49 ns baseline. This is not an across-the-board timing improvement.
- Complete RecordOnly requests have baseline allocations again: **1464 B** with zero groups and **1608 B** with three. Three-group time is **347.98 → 880.76 → 409.91 ns**. Empty sampled requests allocate **1640 B**, versus 1968 B reviewed and 1464 B baseline; creating the sampled scope still costs 176 B even though no cache tags are published.
- Complete sampled requests with three groups cost **1004.66 ns / 3280 B**, versus **902.98 ns / 3784 B** reviewed and **351.16 ns / 1608 B** baseline. The difference from reviewed is **101.68 ns CPU and 504 B less allocation**. The final path serializes bounded JSON and publishes group outcomes instead of creating per-group events. SDK3 capture confirms zero summary MessageData rows. These benchmarks exclude downstream exporter/network cost, so no net end-to-end speedup is claimed for this lifecycle case.
- A separate same-source Off by-key control run measured 17.32 ± 0.15 ns / 0 B. This agrees with the full-matrix result of 17.30 ns; both runs retain zero allocation. Repeat with `--filter '*RequestByKeyHit*Off*'`.

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
| TypedKey | Meter | 12.00 ± 0.28 (0.38) | 41.63 ± 0.54 (0.48) | 12.16 ± 0.23 (0.22) | 1.01× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedKeyEightThreads | Meter | 5.32 ± 0.21 (0.55) | 86.22 ± 1.67 (2.56) | 4.89 ± 0.10 (0.14) | 0.92× | 88 / 88 / 88 | 7.82E-007 / 8.23E-005 / 1.39E-006 |
| TypedHit | Meter | 51.49 ± 0.60 (0.73) | 79.94 ± 0.72 (0.64) | 82.52 ± 0.66 (0.55) | 1.60× | 88 / 160 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| UntypedHit | Meter | 42.27 ± 0.40 (0.33) | 48.62 ± 0.46 (0.43) | 48.98 ± 0.24 (0.19) | 1.16× | 0 / 0 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedHitEightThreads | Meter | 20.34 ± 0.23 (0.19) | 37.06 ± 0.74 (1.97) | 22.03 ± 0.42 (0.39) | 1.08× | 88 / 160 / 88 | 2.09E-007 / 4.17E-007 / 2.68E-007 |
| ExclusiveWarmHit | Meter | 108.03 ± 1.13 (0.88) | 150.77 ± 1.69 (1.58) | 135.16 ± 1.06 (0.94) | 1.25× | 88 / 160 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| PlatformWarmBatch | Meter | 6528.93 ± 53.00 (49.57) | 8500.21 ± 96.28 (85.35) | 8245.95 ± 91.25 (80.89) | 1.26× | 7192 / 10792 / 7192 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestByKeyHit | Meter | 12.20 ± 0.09 (0.07) | 43.34 ± 0.50 (0.41) | 33.92 ± 0.36 (0.34) | 2.78× | 0 / 72 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestWarmBatch | Meter | 2769.05 ± 22.05 (18.41) | 4277.69 ± 77.72 (129.86) | 2510.56 ± 21.90 (19.41) | 0.91× | 1888 / 5488 / 1888 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CrudConstructor | Meter | 30.98 ± 0.29 (0.24) | 60.78 ± 0.79 (0.74) | 36.27 ± 0.32 (0.28) | 1.17× | 48 / 48 / 48 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedKey | Off | 12.43 ± 0.27 (0.50) | 38.58 ± 0.60 (0.56) | 12.54 ± 0.11 (0.10) | 1.01× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedKeyEightThreads | Off | 4.68 ± 0.09 (0.19) | 89.79 ± 1.76 (3.09) | 4.74 ± 0.09 (0.13) | 1.01× | 88 / 88 / 88 | 9.76E-007 / 1.01E-004 / 1.24E-006 |
| TypedHit | Off | 52.15 ± 0.77 (0.68) | 53.67 ± 0.75 (0.67) | 51.44 ± 0.39 (0.34) | 0.99× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| UntypedHit | Off | 51.61 ± 1.03 (1.27) | 51.12 ± 0.25 (0.24) | 44.72 ± 0.22 (0.19) | 0.87× | 0 / 0 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedHitEightThreads | Off | 20.16 ± 0.20 (0.19) | 25.31 ± 0.50 (1.24) | 20.52 ± 0.25 (0.23) | 1.02× | 88 / 88 / 88 | 6.56E-007 / 2.38E-007 / 4.77E-007 |
| ExclusiveWarmHit | Off | 108.09 ± 1.57 (1.47) | 108.77 ± 0.96 (0.90) | 100.62 ± 0.85 (0.79) | 0.93× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| PlatformWarmBatch | Off | 6897.65 ± 51.45 (48.13) | 6985.41 ± 109.02 (101.98) | 6876.35 ± 84.06 (74.51) | 1.00× | 7192 / 7192 / 7192 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestByKeyHit | Off | 12.14 ± 0.14 (0.11) | 44.69 ± 0.64 (0.60) | 17.30 ± 0.28 (0.27) | 1.42× | 0 / 72 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestWarmBatch | Off | 2807.87 ± 39.75 (35.23) | 4052.99 ± 54.91 (48.67) | 2458.52 ± 31.21 (27.66) | 0.88× | 1888 / 5488 / 1888 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CrudConstructor | Off | 30.18 ± 0.32 (0.28) | 61.33 ± 0.54 (0.48) | 35.95 ± 0.44 (0.39) | 1.19× | 48 / 48 / 48 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedKey | Request | 12.19 ± 0.24 (0.22) | 38.75 ± 0.67 (0.63) | 12.07 ± 0.13 (0.11) | 0.99× | 88 / 88 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedKeyEightThreads | Request | 5.10 ± 0.10 (0.13) | 89.60 ± 1.18 (1.11) | 4.46 ± 0.09 (0.11) | 0.87× | 88 / 88 / 88 | 1.20E-006 / 8.82E-005 / 5.59E-007 |
| TypedHit | Request | 53.04 ± 0.37 (0.31) | 101.55 ± 0.95 (0.89) | 96.15 ± 1.09 (1.02) | 1.81× | 88 / 160 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| UntypedHit | Request | 42.66 ± 0.39 (0.34) | 69.24 ± 0.59 (0.52) | 59.71 ± 0.35 (0.31) | 1.40× | 0 / 0 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| TypedHitEightThreads | Request | 20.25 ± 0.21 (0.17) | 138.05 ± 0.84 (0.66) | 61.58 ± 0.30 (0.27) | 3.04× | 88 / 160 / 88 | 8.94E-008 / 4.28E-004 / 4.77E-007 |
| ExclusiveWarmHit | Request | 107.76 ± 0.69 (0.61) | 173.76 ± 5.57 (15.72) | 142.73 ± 0.86 (0.72) | 1.32× | 88 / 160 / 88 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| PlatformWarmBatch | Request | 6586.27 ± 96.95 (90.69) | 9670.05 ± 218.19 (597.29) | 9113.74 ± 60.62 (50.62) | 1.38× | 7192 / 10792 / 7192 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestByKeyHit | Request | 12.29 ± 0.18 (0.23) | 61.11 ± 1.23 (1.15) | 46.71 ± 0.21 (0.19) | 3.80× | 0 / 72 / 0 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| RequestWarmBatch | Request | 2846.39 ± 38.32 (35.85) | 5273.71 ± 51.48 (42.99) | 2495.50 ± 31.95 (29.89) | 0.88× | 1888 / 5488 / 1888 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CrudConstructor | Request | 30.13 ± 0.41 (0.39) | 61.88 ± 1.01 (0.84) | 35.02 ± 0.21 (0.19) | 1.16× | 48 / 48 / 48 | 0.00E+000 / 0.00E+000 / 0.00E+000 |

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
| TypedKey | Meter | 4.90 ± 0.09 (0.17) | 81.65 ± 1.59 (1.49) | 4.81 ± 0.09 (0.24) | 0.98× | 88 / 88 / 88 | 2.95E-004 / 3.52E-004 / 2.94E-004 |
| TypedHitInOneRequest | Meter | 21.05 ± 0.12 (0.11) | 24.69 ± 0.49 (0.56) | 23.36 ± 0.41 (0.70) | 1.11× | 88 / 160 / 88 | 1.98E-004 / 2.09E-004 / 2.31E-004 |
| TypedKey | Off | 5.12 ± 0.15 (0.45) | 83.45 ± 1.02 (0.85) | 4.83 ± 0.10 (0.22) | 0.94× | 88 / 88 / 88 | 2.66E-004 / 3.92E-004 / 2.94E-004 |
| TypedHitInOneRequest | Off | 20.24 ± 0.31 (0.28) | 21.56 ± 0.27 (0.24) | 20.78 ± 0.18 (0.16) | 1.03× | 88 / 88 / 88 | 1.45E-004 / 1.88E-004 / 1.95E-004 |
| TypedKey | Request | 4.92 ± 0.10 (0.27) | 88.56 ± 0.60 (0.50) | 5.03 ± 0.10 (0.23) | 1.02× | 88 / 88 / 88 | 2.75E-004 / 3.95E-004 / 3.02E-004 |
| TypedHitInOneRequest | Request | 20.41 ± 0.18 (0.16) | 141.52 ± 1.58 (1.47) | 63.14 ± 0.33 (0.28) | 3.09× | 88 / 160 / 88 | 1.59E-004 / 6.67E-004 / 2.62E-004 |

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
| CompleteRequest | Recorded&Groups=0 | 202.08 ± 3.60 (3.37) | 319.73 ± 5.72 (4.78) | 268.83 ± 5.15 (5.29) | 1.33× | 1464 / 1968 / 1640 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | Recorded&Groups=3 | 351.16 ± 6.51 (6.09) | 902.98 ± 17.86 (17.54) | 1004.66 ± 19.70 (17.46) | 2.86× | 1608 / 3784 / 3280 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | RecordOnly&Groups=0 | 198.39 ± 2.16 (1.92) | 319.06 ± 6.15 (11.84) | 234.09 ± 4.71 (6.29) | 1.18× | 1464 / 1968 / 1464 | 0.00E+000 / 0.00E+000 / 0.00E+000 |
| CompleteRequest | RecordOnly&Groups=3 | 347.98 ± 6.34 (5.62) | 880.76 ± 16.10 (22.57) | 409.91 ± 8.12 (8.34) | 1.18× | 1608 / 3784 / 1608 | 0.00E+000 / 0.00E+000 / 0.00E+000 |

Gen0 collections per 1,000 operations:

| Method | State | Base | Reviewed | Fixed |
|---|---|---:|---:|---:|
| CompleteRequest | Recorded&Groups=0 | 0.1750 | 0.2351 | 0.1960 |
| CompleteRequest | Recorded&Groups=3 | 0.1922 | 0.4520 | 0.3910 |
| CompleteRequest | RecordOnly&Groups=0 | 0.1750 | 0.2351 | 0.1750 |
| CompleteRequest | RecordOnly&Groups=3 | 0.1922 | 0.4520 | 0.1922 |

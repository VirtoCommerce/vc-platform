# Unified distributed lock (VCST-6052) — design

Jira: https://virtocommerce.atlassian.net/browse/VCST-6052

## Goals

1. One distributed lock interface on Platform level for modules and client solutions.
2. Documented scenarios with code samples.
3. Seamless migration; keep solutions off the lock that Platform uses for startup synchronisation.

## Problems in the current design

| # | Problem | Where |
|---|---|---|
| 1 | Two interfaces named `IDistributedLockService` with different contracts and behaviour | `VirtoCommerce.Platform.Core.DistributedLock`, `VirtoCommerce.Xapi.Core.Infrastructure` |
| 2 | Without Redis the Platform service does not lock at all (`NoLockService`), not even inside one process | `VirtoCommerce.Platform.DistributedLock.NoLock.NoLockService` |
| 3 | `tryLockTimeout` is ignored unless `retryInterval` is also passed; `lockTimeout` reads as a hold limit but is only the crash TTL, because RedLock.net extends held locks automatically | Platform `Redis/DistributedLockService` |
| 4 | `CancellationToken?` instead of `CancellationToken cancellationToken = default`; the token never reaches the protected code | Platform |
| 5 | Lock failure is a generic `PlatformException` (Platform) or `LockError : GraphQL.ExecutionError` (XAPI) | both |
| 6 | Only `Func<T>`/`Func<Task<T>>` delegates; no non-generic overload; no way to hold a lock across statements; sync API | both |
| 7 | XAPI hard-codes 20 s expiry / 10 s wait / 2 s retry, has no cancellation, and creates a `RedLockFactory` per call | `VirtoCommerce.Xapi.Data.Services.DistributedLockService` |
| 8 | Keys are `redlock:{resource}` with no application prefix; applications sharing Redis block each other | RedLock.net |
| 9 | The startup lock `IInternalDistributedLockService` is public in the `VirtoCommerce.Platform.DistributedLock` package | `LocalStorageModuleCatalog`, `ApplicationBuilderExtensions.ExecuteSynchronized` |

## Current consumers

| Abstraction | Call sites | Repositories |
|---|---|---|
| Platform `IDistributedLockService` | 8, all `ExecuteAsync` | background-jobs, elastic-search-8, elastic-search-9, image-tools, loyalty, order, search |
| XAPI `IDistributedLockService` | 4 direct + 39 `ResolveSynchronizedAsync` | x-cart, ucp |
| Legacy experience-api `IDistributedLockService` | 36 `ResolveSynchronizedAsync` | experience-api (deprecated, out of scope) |
| `IInternalDistributedLockService` | 2 | vc-platform only |

Callers that depend on the failure type: background-jobs, image-tools and search catch `PlatformException`; UCP catches `LockError`. Order, search and x-cart build with `TreatWarningsAsErrors`.

## Decisions

1. The existing Platform `IDistributedLockService` switches to the new implementation, including the in-process lock when Redis is not configured.
2. `DistributedLock:KeyPrefix` defaults to empty, so lock keys do not change during a rolling deploy.
3. Names: `IDistributedLock`, `IDistributedLockHandle`, `DistributedLockTimeoutException`, `DistributedLockUnavailableException`.
4. Scope: Platform, XAPI and UCP. Out of scope: experience-api, the 23 in-process `AsyncLock` call sites that intend cross-instance exclusion (cart, order, fast-order, webhooks, Platform settings), and Hangfire `[DisableConcurrentExecution]`.
5. The Platform `IDistributedLockService` is deprecated in XML docs only: order and search treat warnings as errors. The XAPI `IDistributedLockService` is `[Obsolete(VC0015)]` (x-cart migrates in the same release). The startup lock has no callers outside vc-platform, so it gets `[Obsolete]` and `[EditorBrowsable(Never)]`. New obsolete members use `DiagnosticId = "VC0015"`.
6. `Startup.Configure` takes the `Startup` lock with `IDistributedLock.Acquire` (same resource name, so the Redis key `redlock:Startup` is unchanged while `KeyPrefix` is empty), waiting `DistributedLock:WaitTime`. `ApplicationBuilderExtensions.ExecuteSynchronized` is removed; Platform.Web is not a published package.
7. Synchronous extension methods `Acquire`, `TryAcquire`, `Execute`, `Execute<T>` and `TryExecute` mirror the async API. They are extensions rather than interface members, so implementations stay async only; they block the calling thread and are documented for startup, console tools and synchronous legacy APIs.
8. "Held elsewhere" and "lock store unavailable" are different outcomes: RedLock reports every Redis error as `NoQuorum`, which throws `DistributedLockUnavailableException` from both `Acquire*` and `TryAcquire*`; `null` / `DistributedLockTimeoutException` mean contention only.
9. `IDistributedLockHandle.HandleLostToken` is part of the contract from the first release, so third-party backends implement it from the start. Redis cancels it when renewal fails or no renewal succeeded for a whole `Expiry`; in-process returns `CancellationToken.None`. `ExecuteAsync` / `TryExecuteAsync` link it into the action's token.
10. Acquisition is our own loop of single RedLock attempts (factory with `retryCount: 1`): a zero timeout is one attempt, the token is honoured between attempts, and the delay backs off 1.8× with ±20% jitter from `RetryInterval` to `MaxRetryInterval`.
11. The startup lock uses a keyed `IDistributedLock` with `DistributedLock:StartupExpiry` (5 min, the previous `120 s + WaitTime` lease), so a Redis brownout during migrations does not let a second instance start.
12. Telemetry names are fixed before release: span outcome on every path (`acquired`, `timeout`, `cancelled`, `unavailable`, `error`), durations in seconds, and a `Meter` (`vc.lock.attempts`, `vc.lock.wait.duration`, `vc.lock.hold.duration`, `vc.lock.lost`) tagged by resource family.
13. The lock is documented as an efficiency lock: it never replaces a unique constraint, optimistic concurrency or an idempotency key.

## API (`VirtoCommerce.Platform.Core.DistributedLock`)

```csharp
public interface IDistributedLock
{
    // Waits up to timeout (default DistributedLock:DefaultTimeout).
    // Throws DistributedLockTimeoutException, or OperationCanceledException when cancelled.
    Task<IDistributedLockHandle> AcquireAsync(string resource, TimeSpan? timeout = null, CancellationToken cancellationToken = default);

    // Returns null when not acquired within timeout. Default: try once, do not wait.
    Task<IDistributedLockHandle?> TryAcquireAsync(string resource, TimeSpan timeout = default, CancellationToken cancellationToken = default);
}

public interface IDistributedLockHandle : IAsyncDisposable, IDisposable
{
    string Resource { get; }
    CancellationToken HandleLostToken { get; } // cancelled when the lock is lost while held
}

public sealed class DistributedLockUnavailableException : PlatformException // Redis unreachable (NoQuorum)
{
    public string? Resource { get; }
}

public sealed class DistributedLockTimeoutException : PlatformException
{
    // Standard constructors (), (message), (message, inner), plus (resource, timeout) and (resource, timeout, inner).
    public string? Resource { get; }
    public TimeSpan Timeout { get; }
}

public static class DistributedLockExtensions
{
    Task<T> ExecuteAsync<T>(this IDistributedLock, string resource, Func<CancellationToken, Task<T>> action, TimeSpan? timeout = null, CancellationToken cancellationToken = default);
    Task ExecuteAsync(this IDistributedLock, string resource, Func<CancellationToken, Task> action, TimeSpan? timeout = null, CancellationToken cancellationToken = default);
    Task<bool> TryExecuteAsync(this IDistributedLock, string resource, Func<CancellationToken, Task> action, TimeSpan timeout = default, CancellationToken cancellationToken = default);

    // Blocking counterparts for synchronous code.
    IDistributedLockHandle Acquire(this IDistributedLock, string resource, TimeSpan? timeout = null, CancellationToken cancellationToken = default);
    IDistributedLockHandle? TryAcquire(this IDistributedLock, string resource, TimeSpan timeout = default, CancellationToken cancellationToken = default);
    void Execute(this IDistributedLock, string resource, Action action, TimeSpan? timeout = null, CancellationToken cancellationToken = default);
    T Execute<T>(this IDistributedLock, string resource, Func<T> action, TimeSpan? timeout = null, CancellationToken cancellationToken = default);
    bool TryExecute(this IDistributedLock, string resource, Action action, TimeSpan timeout = default, CancellationToken cancellationToken = default);
}
```

Rules:

- `resource` must not be null or white space; `timeout` must be non-negative or `Timeout.InfiniteTimeSpan` (wait until acquired or cancelled).
- Locks are not reentrant: acquiring the same resource twice in one flow waits for itself.
- Resource names follow `{module}:{entity}:{id}`, for example `cart:recalc:{cartId}`. They appear in Redis keys and exception messages, so they must not contain secrets. Traces carry only a hash, because names often contain user or entity ids.
- Implementations are async only. Synchronous callers use the blocking extension methods (`Acquire`, `TryAcquire`, `Execute`, `TryExecute`), which are intended for startup and synchronous legacy code.
- `DistributedLockTimeoutException` derives from `PlatformException`, so existing `catch (PlatformException)` blocks keep working.
- New files enable nullable reference types, so `TryAcquireAsync` / `TryAcquire` are annotated as returning `null` on contention.
- Library awaits use `ConfigureAwait(false)`, so the blocking extensions do not deadlock under a `SynchronizationContext`.

## Configuration (`DistributedLock` section, `DistributedLockOptions`)

| Key | Default | Meaning |
|---|---|---|
| `WaitTime` | `180` | Existing. Seconds the startup lock waits. |
| `DefaultTimeout` | `00:00:30` | Wait used by `AcquireAsync` without an explicit timeout. |
| `Expiry` | `00:00:30` | Redis lease, renewed every `Expiry / 2`. Bounds how long a crashed holder blocks others, and how long renewal may fail before a live holder loses the lock. |
| `RetryInterval` | `00:00:00.1` | First delay between acquisition attempts; grows 1.8× with ±20% jitter. |
| `MaxRetryInterval` | `00:00:02` | Upper bound for that delay. |
| `StartupExpiry` | `00:05:00` | Lease of the startup lock (migrations, module `PostInitialize`). |
| `KeyPrefix` | empty | Optional application prefix: key becomes `redlock:{KeyPrefix}:{resource}`. Changing it during a rolling deploy breaks mutual exclusion between old and new instances. |

## Implementations (`VirtoCommerce.Platform.DistributedLock`)

- `DistributedLockBase` — validation, default timeout, timeout exception, tracing (`ActivitySource` `VirtoCommerce.Platform.DistributedLock`, span `DistributedLock acquire` with `vc.lock.resource_hash`, `vc.lock.resource_family`, `vc.lock.outcome`, `vc.lock.wait.duration` in seconds) and metrics (`Meter` of the same name), plus a handle wrapper that records hold time and loss.
- `RedisDistributedLock` — loop of single RedLock attempts on a dedicated single-attempt factory; `NoQuorum` throws `DistributedLockUnavailableException`; the handle polls `IsAcquired` and `ExtendCount` every `Expiry / 4` (100 ms–5 s) to raise `HandleLostToken`. Both RedLock factories get the Platform `ILoggerFactory`, so failed renewals are logged.
- `InProcessDistributedLock` — per-resource `SemaphoreSlim` with reference-counted eviction. Public so tests can use a real lock.
- `DistributedLockServiceAdapter` — implements the existing Platform `IDistributedLockService` on `IDistributedLock`. `tryLockTimeout` becomes the wait (`null`, zero or negative = one attempt, as before). `lockTimeout` and `retryInterval` are ignored because expiry and retry are configured once.
- Registration in `AddRedis`: `IDistributedLock` is `RedisDistributedLock` when `ConnectionStrings:RedisConnectionString` is set, otherwise `InProcessDistributedLock`. `IDistributedLockService` is always `DistributedLockServiceAdapter`. `NoLockService` and the old Redis `DistributedLockService` are no longer registered and get `[Obsolete]`.

## Behaviour changes for existing callers

| Change | Who notices |
|---|---|
| Without Redis, `IDistributedLockService` now serializes callers inside one instance | Single-instance installs: concurrent indexing, thumbnail or loyalty-balance calls now wait or fail fast instead of running in parallel |
| `lockTimeout` no longer sets the Redis TTL; the configured `Expiry` (30 s, auto-extended) does | image-tools (8 h), search (12 h), order payments (30 min): after a crash the stale lock clears in about 30 s instead of hours |
| `tryLockTimeout` without `retryInterval` now waits instead of failing immediately | No current caller; all pass both |
| Failure message text changes; the type is `DistributedLockTimeoutException : PlatformException` | Nobody matches on the message |
| Startup synchronization uses a keyed `IDistributedLock` with `StartupExpiry` (5 min, as before): waits up to `WaitTime` with backoff from 100 ms instead of a fixed 3 s; without Redis it takes the in-process lock | Multi-instance startups poll Redis more often at first, then back off |
| Redis unreachable: `IDistributedLockService` throws `DistributedLockUnavailableException` (a `PlatformException`) immediately, instead of a `PlatformException` after the wait | Callers catching `PlatformException` see the same type, sooner |
| XAPI keeps its 10 s wait (`VirtoCommerce:GraphQLDistributedLock:Timeout`), retries with backoff from 100 ms instead of every 2 s, and still throws `LockError` (`ServiceAccessLocked`) | Faster acquisition under contention; same GraphQL error |

## Scenarios

1. Serialize changes to one entity — `ExecuteAsync($"cart:recalc:{cartId}", ...)`.
2. Consume a single-use token — the atomic consume in the token store decides single use (`GETDEL`, conditional `UPDATE`); `TryAcquireAsync(key, TimeSpan.FromSeconds(10))` only narrows the race, and `null` means a retryable 409.
3. Skip if another instance runs it — `TryExecuteAsync("catalog:reindex", ...)`; `false` means skipped.
4. Hold a lock across several steps (for example a multi-step export) — `await using (var handle = await AcquireAsync(...))`, linking `handle.HandleLostToken` into the work.
5. Unit tests — `new InProcessDistributedLock(Options.Create(new DistributedLockOptions()), NullLogger<InProcessDistributedLock>.Instance)`.
6. GraphQL resolvers — XAPI `ResolveSynchronizedAsync(prefix, property, IDistributedLock, resolve)` maps a timeout to `LockError`.

## Alternatives considered

[madelson/DistributedLock](https://github.com/madelson/DistributedLock) (MIT, active) was evaluated in three shapes:

| | A. VC API + RedLock.net (chosen) | B. VC API + DistributedLock backend | C. DistributedLock API in module code |
|---|---|---|---|
| Public API owned by VC | Yes | Yes | No |
| New dependency | None; RedLock.net already ships | DistributedLock.Core, .Redis, optionally .SqlServer/.Postgres/.MySql | Same as B, referenced by every module |
| Cross-instance lock without Redis | No; in-process only | Yes, through the shared database | Yes |
| StackExchange.Redis 3.0 | In use today | Not verified: DistributedLock.Redis 1.1.1 is built against 2.7.33 | Not verified |
| Migration for modules | Seamless | Seamless; only the implementation changes | Breaking |

A was chosen: the problems in this ticket are in the VC API, and B keeps the same `IDistributedLock` contract, so it can follow without affecting callers. C would make third-party types part of the Platform contract and clashes with the `IDistributedLock` name.

Follow-up: evaluate a database-backed `IDistributedLock` (DistributedLock.SqlServer, .Postgres, .MySql) for multi-instance installs without Redis, starting with a spike that confirms DistributedLock.Redis on StackExchange.Redis 3.0.

## Delivery

1. vc-platform — new API, implementations, adapter, registration, startup-lock deprecation, docs (`docs/fundamentals/distributed-lock.md`).
2. vc-module-x-api — XAPI `IDistributedLockService` becomes an obsolete adapter over `IDistributedLock`; `ResolveSynchronized*` overloads and `AcquireForGraphQLAsync` taking `IDistributedLock`; `VirtoCommerce:GraphQLDistributedLock:Timeout` (10 s).
3. vc-module-ucp — handoff restore uses `IDistributedLock.TryAcquireAsync`.
4. vc-module-x-cart — cart locks use `IDistributedLock`; the obsolete `IMediator` constructors of the lock classes are removed (keeping them made DI constructor selection ambiguous).

Each step releases before the next starts: XAPI needs the Platform package, UCP needs both.

Follow-ups: the 23 in-process `AsyncLock` call sites that intend cross-instance exclusion; semaphore and reader-writer primitives; a leader-election helper built on `HandleLostToken`; a database-backed backend.

# Distributed lock

Use `IDistributedLock` (`VirtoCommerce.Platform.Core.DistributedLock`) when code should not run concurrently for the same resource, across all platform instances.

- With `ConnectionStrings:RedisConnectionString`, locks are held in Redis and exclude every instance.
- Without Redis, locks serialize callers inside the current instance only.
- Locks are not reentrant. Resource names follow `{module}:{entity}:{id}`. They appear in Redis keys, logs and exception messages, so never put secrets in them. Traces and metrics record only a hash of the name and its family (the name without the last segment).
- Pass `Timeout.InfiniteTimeSpan` to wait until the lock is acquired or the cancellation token is cancelled.

## What the lock guarantees

This is an **efficiency lock**: it stops instances from doing the same work at the same time. It is not a correctness guarantee. On a single Redis it does not guarantee mutual exclusion across a replica failover (replication is asynchronous), a process pause longer than the lease, or a lock lost because renewal failed.

So a lock never replaces a unique constraint, optimistic concurrency or an idempotency key; it only makes conflicts rarer. Keep the correctness rule in the data store, and use the lock to avoid wasted or duplicate work.

Further reading: Martin Kleppmann, [How to do distributed locking](https://martin.kleppmann.com/2016/02/08/how-to-do-distributed-locking.html), and Redis, [Distributed locks with Redis](https://redis.io/docs/latest/develop/clients/patterns/distributed-locks/).

## Scenarios

### Serialize changes to one entity

```csharp
await _distributedLock.ExecuteAsync($"cart:recalc:{cartId}",
    ct => RecalculateAndSaveAsync(cartId, ct), TimeSpan.FromSeconds(5), cancellationToken);
```

The token passed to the action is cancelled when `cancellationToken` is cancelled or the lock is lost.

### Skip work that another instance is already doing

```csharp
if (!await _distributedLock.TryExecuteAsync("catalog:reindex", ReindexAsync, cancellationToken: cancellationToken))
{
    _logger.LogInformation("Reindex is already running on another instance; skipped.");
}
```

`false` always means another holder. If Redis is unreachable, `TryExecuteAsync` throws `DistributedLockUnavailableException` instead, so an outage is never mistaken for "already running".

### Hold a lock across several steps

```csharp
await using (var handle = await _distributedLock.AcquireAsync($"export:catalog:{catalogId}", TimeSpan.FromSeconds(10), cancellationToken))
{
    using var lockScope = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, handle.HandleLostToken);

    var file = await WriteExportFileAsync(catalogId, lockScope.Token);
    await UploadAsync(file, lockScope.Token);
}

await NotifyAsync(cancellationToken);
```

`HandleLostToken` is cancelled when the lock is lost while held (see Failures), so long work stops instead of continuing next to a second holder.

### Consume a single-use token

Single use comes from the token store, not from the lock: consume the token atomically, for example with Redis `GETDEL`, or `UPDATE … SET ConsumedAt = @now WHERE Key = @key AND ConsumedAt IS NULL` and a row-count check. The lock only makes the race rarer.

```csharp
await using var handle = await _distributedLock.TryAcquireAsync($"ucp:handoff:{keyHash}", TimeSpan.FromSeconds(10), cancellationToken);
if (handle is null)
{
    return Conflict(); // retryable: another request is still restoring the token
}

// Atomic consume: returns null when another request already used the token.
var session = await _sessions.ConsumeAsync(keyHash, cancellationToken);
return session is null
    ? BadRequest("The token was already used.") // not retryable
    : Ok(session);
```

The loser of the race usually gets the lock right after the winner consumed the token, so the atomic consume, not the lock, decides the answer.

### Synchronous code

`Acquire`, `TryAcquire`, `Execute` and `TryExecute` are blocking counterparts of the async methods. They hold a thread while waiting, so use them only where no async path exists: application startup, console tools, or synchronous legacy APIs. In request handlers and background jobs, use the async methods.

Platform startup runs migrations and module `PostInitialize` one instance at a time, with its own longer lease (`DistributedLock:StartupExpiry`):

```csharp
var distributedLock = app.ApplicationServices.GetRequiredKeyedService<IDistributedLock>(ServiceCollectionExtensions.StartupLockServiceKey);

using (distributedLock.Acquire("Startup", TimeSpan.FromSeconds(180)))
{
    app.UsePlatformMigrations(configuration);
    ModuleBootstrapper.Instance.PostInitializeModules(app);
}
```

Run a synchronous action, or skip it when another instance holds the lock:

```csharp
distributedLock.Execute($"export:{jobId}", () => WriteExportFile(jobId), TimeSpan.FromSeconds(30));

if (!distributedLock.TryExecute("cleanup:temp-files", DeleteTempFiles))
{
    logger.LogInformation("Cleanup is already running on another instance; skipped.");
}
```

### Unit tests

Use the in-process implementation instead of a mock:

```csharp
var distributedLock = new InProcessDistributedLock(
    Options.Create(new DistributedLockOptions()),
    NullLogger<InProcessDistributedLock>.Instance);
```

It lives in the `VirtoCommerce.Platform.DistributedLock` package, which also brings StackExchange.Redis and RedLock.net. Most module test projects reference only `VirtoCommerce.Platform.Core`; there, a small fake `IDistributedLock` avoids the extra dependencies.

## Choosing a timeout

`DistributedLock:DefaultTimeout` (30 s) applies only when no timeout is passed. Pass one that fits the caller:

| Caller | Recommendation |
|---|---|
| Request path (GraphQL, API) | A few seconds, and a retryable error when it expires. XAPI resolvers use `VirtoCommerce:GraphQLDistributedLock:Timeout` (10 s). |
| Background job that must run once per cluster | `TryExecuteAsync` with no wait: skip when another instance runs it. |
| Background job that must run eventually | An explicit wait, or `Timeout.InfiniteTimeSpan` with the job's cancellation token. |

## Failures

| Situation | Result |
|---|---|
| `AcquireAsync` / `ExecuteAsync` / `Acquire` / `Execute`: another holder kept the lock for the whole timeout | `DistributedLockTimeoutException` (derives from `PlatformException`, exposes `Resource` and `Timeout`) |
| `TryAcquireAsync` / `TryExecuteAsync` / `TryAcquire` / `TryExecute`: another holder kept the lock for the whole timeout | `null` / `false` |
| Redis cannot be reached (any of the methods) | `DistributedLockUnavailableException` (derives from `PlatformException`), right away rather than after the timeout |
| The cancellation token is cancelled while waiting | `OperationCanceledException` |
| The lock is lost while held: renewal failed for a whole `Expiry` | `IDistributedLockHandle.HandleLostToken` is cancelled; the token passed to `ExecuteAsync` / `TryExecuteAsync` actions is cancelled too |

A holder learns about a loss when its renewal fails. On a dropped connection a renewal can block for the StackExchange.Redis `syncTimeout` (5 s by default), and the handle also treats a whole `Expiry` without a successful renewal as lost. With the defaults (renewal every 15 s, 30 s lease) the holder is told before another instance can take the lock.

## Configuration

```json
"DistributedLock": {
  "DefaultTimeout": "00:00:30",
  "Expiry": "00:00:30",
  "RetryInterval": "00:00:00.1",
  "MaxRetryInterval": "00:00:02",
  "StartupExpiry": "00:05:00",
  "KeyPrefix": ""
}
```

| Key | Default | Meaning |
|---|---|---|
| `DefaultTimeout` | `00:00:30` | Wait for `AcquireAsync`, `ExecuteAsync`, `Acquire` and `Execute` without an explicit timeout |
| `Expiry` | `00:00:30` | Redis lock lease, renewed every `Expiry / 2` while held. It bounds two things: how long a crashed holder blocks others, and how long renewal may fail (for example during a Redis brownout) before a live holder loses the lock |
| `RetryInterval` | `00:00:00.1` | First delay between Redis acquisition attempts while waiting. Later delays grow by 1.8×, with ±20% jitter, so waiters do not poll a busy key in lockstep |
| `MaxRetryInterval` | `00:00:02` | Upper bound for that delay |
| `StartupExpiry` | `00:05:00` | Lease of the startup lock that serializes migrations and module `PostInitialize`; longer than `Expiry` so a brownout during a long migration does not let a second instance start migrating |
| `KeyPrefix` | empty | Isolates applications sharing one Redis: keys become `redlock:{KeyPrefix}:{resource}`. Changing it during a rolling deploy breaks mutual exclusion between old and new instances |
| `WaitTime` | `180` | Seconds each instance waits for the startup lock |

## Tracing and metrics

Both use the name `VirtoCommerce.Platform.DistributedLock` (`ActivitySource` and `Meter`).

Each acquisition emits a `DistributedLock acquire` span with:
- `vc.lock.resource_hash`: the first 16 hex characters of the SHA-256 of the resource name. To find the spans for a known resource, compute the same hash;
- `vc.lock.resource_family`: the resource without its last segment, for example `cart:recalc`;
- `vc.lock.outcome`: `acquired`, `timeout`, `cancelled`, `unavailable` or `error`; the span status is `Error` for the last three;
- `vc.lock.wait.duration`: seconds spent acquiring.

Metrics, tagged with `vc.lock.resource_family` (and `vc.lock.outcome` where noted):

| Instrument | Unit | Meaning |
|---|---|---|
| `vc.lock.attempts` | `{attempt}` | Acquisitions, by outcome |
| `vc.lock.wait.duration` | `s` | Time spent acquiring, by outcome |
| `vc.lock.hold.duration` | `s` | Time from acquisition to release |
| `vc.lock.lost` | `{lock}` | Locks lost while held |

## Migrating from `IDistributedLockService`

`VirtoCommerce.Platform.Core.DistributedLock.IDistributedLockService` is deprecated and now runs on `IDistributedLock`:

| Old parameter | Now |
|---|---|
| `tryLockTimeout` | The wait; `null`, zero or negative tries once, as before |
| `lockTimeout` | Ignored; `DistributedLock:Expiry` with automatic renewal applies |
| `retryInterval` | Ignored; `DistributedLock:RetryInterval` and `MaxRetryInterval` apply |
| `cancellationToken` | Cancels the wait |

```csharp
// Before
await _distributedLockService.ExecuteAsync($"loyalty-balance:{userId}", () => UpdateBalanceAsync(userId),
    TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30), TimeSpan.FromMilliseconds(200), cancellationToken);

// After
await _distributedLock.ExecuteAsync($"loyalty-balance:{userId}", ct => UpdateBalanceAsync(userId, ct),
    TimeSpan.FromSeconds(30), cancellationToken);
```

Without Redis, the old interface now serializes callers inside the instance instead of running them concurrently. When Redis is unreachable it throws `DistributedLockUnavailableException`, which derives from `PlatformException` like the previous failure.

`IInternalDistributedLockService` and `DistributedLockCondition` are reserved for platform startup and are marked obsolete (`VC0015`); do not use them in modules. The Platform.Web `ExecuteSynchronized` extension was removed; startup uses `IDistributedLock.Acquire`.

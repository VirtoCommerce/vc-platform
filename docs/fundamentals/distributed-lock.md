# Distributed lock

Use `IDistributedLock` (`VirtoCommerce.Platform.Core.DistributedLock`) when code must not run concurrently for the same resource, across all platform instances.

- With `ConnectionStrings:RedisConnectionString`, locks are held in Redis and exclude every instance.
- Without Redis, locks serialize callers inside the current instance only.
- Locks are not reentrant. Resource names follow `{module}:{entity}:{id}`. They appear in Redis keys and in `DistributedLockTimeoutException` messages, so never put secrets in them; traces record only a hash of the name.
- Pass `Timeout.InfiniteTimeSpan` to wait until the lock is acquired or the cancellation token is cancelled.

## Scenarios

### Serialize changes to one entity

```csharp
await _distributedLock.ExecuteAsync($"cart:recalc:{cartId}",
    ct => RecalculateAndSaveAsync(cartId, ct), cancellationToken: cancellationToken);
```

`ExecuteAsync` waits up to `DistributedLock:DefaultTimeout` (10 s) and throws `DistributedLockTimeoutException` if the lock stays busy.

### Consume a single-use token

```csharp
await using var handle = await _distributedLock.TryAcquireAsync($"ucp:handoff:{keyHash}", TimeSpan.FromSeconds(10), cancellationToken);
if (handle is null)
{
    return Conflict(); // retryable: another request is using the token
}

return await ConsumeTokenAsync(keyHash, cancellationToken);
```

### Skip work that another instance is already doing

```csharp
if (!await _distributedLock.TryExecuteAsync("catalog:reindex", ReindexAsync, cancellationToken: cancellationToken))
{
    _logger.LogInformation("Reindex is already running on another instance; skipped.");
}
```

### Hold a lock across several steps and release it early

```csharp
await using (await _distributedLock.AcquireAsync($"order:number:{storeId}", TimeSpan.FromSeconds(30), cancellationToken))
{
    var next = await ReserveNumberAsync(storeId, cancellationToken);
    await SaveCounterAsync(storeId, next, cancellationToken);
}

await NotifyAsync(cancellationToken);
```

### Synchronous code

`Acquire`, `TryAcquire`, `Execute` and `TryExecute` are blocking counterparts of the async methods. They hold a thread while waiting, so use them only where no async path exists: application startup, console tools, or synchronous legacy APIs. In request handlers and background jobs, use the async methods.

Platform startup runs migrations and module `PostInitialize` one instance at a time:

```csharp
var distributedLock = app.ApplicationServices.GetRequiredService<IDistributedLock>();

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

## Failures

| Situation | Result |
|---|---|
| `AcquireAsync` / `ExecuteAsync` / `Acquire` / `Execute` cannot get the lock in time | `DistributedLockTimeoutException` (derives from `PlatformException`, exposes `Resource` and `Timeout`) |
| `TryAcquireAsync` / `TryExecuteAsync` / `TryAcquire` / `TryExecute` cannot get the lock in time | `null` / `false` |
| The cancellation token is cancelled while waiting | `OperationCanceledException` |

## Configuration

```json
"DistributedLock": {
  "DefaultTimeout": "00:00:10",
  "Expiry": "00:00:30",
  "RetryInterval": "00:00:00.1",
  "KeyPrefix": ""
}
```

| Key | Default | Meaning |
|---|---|---|
| `DefaultTimeout` | `00:00:10` | Wait for `AcquireAsync`, `ExecuteAsync`, `Acquire` and `Execute` without an explicit timeout |
| `Expiry` | `00:00:30` | Redis lock TTL. Extended automatically while held; bounds only how long a crashed holder blocks others |
| `RetryInterval` | `00:00:00.1` | Interval between Redis acquisition attempts while waiting |
| `KeyPrefix` | empty | Isolates applications sharing one Redis: keys become `redlock:{KeyPrefix}:{resource}`. Changing it during a rolling deploy breaks mutual exclusion between old and new instances |
| `WaitTime` | `180` | Seconds each instance waits for the `Startup` lock that serializes migrations and module `PostInitialize` |

## Tracing

Each acquisition emits a `DistributedLock acquire` span from the `VirtoCommerce.Platform.DistributedLock` activity source, with `vc.lock.resource_hash` (the first 16 hex characters of the SHA-256 of the resource name), `vc.lock.outcome` (`acquired`, `timeout`, `cancelled`) and `vc.lock.wait_ms`. Resource names often contain user or entity ids, so the name itself is not exported. To find the spans for a known resource, compute the same hash.

## Migrating from `IDistributedLockService`

`VirtoCommerce.Platform.Core.DistributedLock.IDistributedLockService` is deprecated and now runs on `IDistributedLock`:

| Old parameter | Now |
|---|---|
| `tryLockTimeout` | The wait; `null` tries once |
| `lockTimeout` | Ignored; `DistributedLock:Expiry` with automatic extension applies |
| `retryInterval` | Ignored; `DistributedLock:RetryInterval` applies |
| `cancellationToken` | Cancels the wait |

```csharp
// Before
await _distributedLockService.ExecuteAsync($"loyalty-balance:{userId}", () => UpdateBalanceAsync(userId),
    TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30), TimeSpan.FromMilliseconds(200), cancellationToken);

// After
await _distributedLock.ExecuteAsync($"loyalty-balance:{userId}", ct => UpdateBalanceAsync(userId, ct),
    TimeSpan.FromSeconds(30), cancellationToken);
```

Without Redis, the old interface now serializes callers inside the instance instead of running them concurrently.

`IInternalDistributedLockService` and `DistributedLockCondition` are reserved for platform startup and are marked obsolete (`VC0015`); do not use them in modules. The Platform.Web `ExecuteSynchronized` extension was removed; startup uses `IDistributedLock.Acquire`.

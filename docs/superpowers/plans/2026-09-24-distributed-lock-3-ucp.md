# Distributed lock — UCP implementation plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Serialize UCP handoff restore through the Platform `IDistributedLock` instead of the XAPI `IDistributedLockService`.

**Architecture:** `UcpCheckoutService.RestoreHandoff` takes the lock with `TryAcquireAsync` and a 10 second wait; `null` becomes the existing retryable `409 handoff_in_progress`. `VirtoCommerce.UCP.Data` then no longer needs `VirtoCommerce.Xapi.Core`.

**Tech Stack:** .NET 10, xUnit v3.

Spec: `vc-platform/docs/superpowers/specs/2026-09-24-unified-distributed-lock-design.md`.

Prerequisites: Platform `3.1073.0` (plan 1) and the XAPI release from plan 2. Check with `gh release list --repo VirtoCommerce/vc-platform --limit 5` and `gh release list --repo VirtoCommerce/vc-module-x-api --limit 5`; this plan writes them as `3.1073.0` and `<XAPI_VERSION>` — substitute the released XAPI version (for example `3.1024.0`) before editing.

Repository: `C:\Projects\git\VirtoCommerce\vc-module-ucp`.

---

## File structure

| File | Responsibility |
|---|---|
| Modify `src/VirtoCommerce.UCP.Core/VirtoCommerce.UCP.Core.csproj`, `src/VirtoCommerce.UCP.Data/VirtoCommerce.UCP.Data.csproj`, `src/VirtoCommerce.UCP.ExperienceApi/VirtoCommerce.UCP.ExperienceApi.csproj`, `src/VirtoCommerce.UCP.Web/module.manifest` | Platform and XAPI versions; drop `Xapi.Core` from Data |
| Modify `src/VirtoCommerce.UCP.Data/Services/UcpCheckoutService.cs` | Restore under `IDistributedLock` |
| Modify `tests/VirtoCommerce.UCP.Tests/UcpCheckoutServiceTests.cs` | Test double and busy test |
| Modify `README.md:86` | Lock description |

---

### Task 1: Branch and versions

- [ ] **Step 1: Create the branch**

```bash
cd C:/Projects/git/VirtoCommerce/vc-module-ucp
git status --short
git fetch origin
git switch -c feat/VCST-6052 origin/dev
```

If PR #7 (`feat/VCST-5378-unified-buyer-flow`) is not merged into `dev` yet, branch from it instead: `git switch -c feat/VCST-6052 origin/feat/VCST-5378-unified-buyer-flow`. `UcpCheckoutService.RestoreHandoff` must contain the `catch (LockError exception)` block before you continue.

- [ ] **Step 2: Update versions**

In `src/VirtoCommerce.UCP.Core/VirtoCommerce.UCP.Core.csproj` and `src/VirtoCommerce.UCP.Data/VirtoCommerce.UCP.Data.csproj`, set:

```xml
    <PackageReference Include="VirtoCommerce.Platform.Core" Version="3.1073.0" />
```

In `src/VirtoCommerce.UCP.Data/VirtoCommerce.UCP.Data.csproj`, delete:

```xml
    <PackageReference Include="VirtoCommerce.Xapi.Core" Version="3.1015.0" />
```

In `src/VirtoCommerce.UCP.ExperienceApi/VirtoCommerce.UCP.ExperienceApi.csproj`, set `VirtoCommerce.Xapi.Core` to `<XAPI_VERSION>`.

In `src/VirtoCommerce.UCP.Web/module.manifest`, set `<platformVersion>3.1073.0</platformVersion>` and `<dependency id="VirtoCommerce.Xapi" version="<XAPI_VERSION>" />`. Then open the XAPI release's `module.manifest` (`gh api repos/VirtoCommerce/vc-module-x-api/contents/src/VirtoCommerce.Xapi.Web/module.manifest?ref=<XAPI_VERSION> --jq .content | base64 -d`) and raise any UCP dependency that is lower than what XAPI requires (for example `VirtoCommerce.Store`).

Run: `dotnet build`
Expected: the build fails only in `UcpCheckoutService.cs` and `UcpCheckoutServiceTests.cs` with `The type or namespace name 'Xapi' does not exist`. Any `NU1605` downgrade error: raise the named package to the version NuGet reports.

---

### Task 2: Restore under `IDistributedLock`

**Files:**
- Modify: `src/VirtoCommerce.UCP.Data/Services/UcpCheckoutService.cs`
- Modify: `tests/VirtoCommerce.UCP.Tests/UcpCheckoutServiceTests.cs`

- [ ] **Step 1: Replace the test double**

In `tests/VirtoCommerce.UCP.Tests/UcpCheckoutServiceTests.cs`, replace `using VirtoCommerce.Xapi.Core.Infrastructure;` with `using VirtoCommerce.Platform.Core.DistributedLock;`, and replace the whole `TestDistributedLockService` class with:

```csharp
    private sealed class TestDistributedLock : IDistributedLock
    {
        public ConcurrentQueue<string> ResourceKeys { get; } = new();

        public bool IsBusy { get; init; }

        public Task<IDistributedLockHandle> AcquireAsync(string resource, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            ResourceKeys.Enqueue(resource);
            return IsBusy
                ? Task.FromException<IDistributedLockHandle>(new DistributedLockTimeoutException(resource, timeout ?? TimeSpan.FromSeconds(10)))
                : Task.FromResult<IDistributedLockHandle>(new Handle(resource));
        }

        public Task<IDistributedLockHandle> TryAcquireAsync(string resource, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            ResourceKeys.Enqueue(resource);
            return Task.FromResult<IDistributedLockHandle>(IsBusy ? null : new Handle(resource));
        }

        private sealed class Handle(string resource) : IDistributedLockHandle
        {
            public string Resource { get; } = resource;

            public void Dispose()
            {
            }

            public ValueTask DisposeAsync()
            {
                return ValueTask.CompletedTask;
            }
        }
    }
```

Rename every `new TestDistributedLockService(` to `new TestDistributedLock(`, and in `CreateService` change the parameter `IDistributedLockService distributedLock = null` to `IDistributedLock distributedLock = null`.

In `RestoreHandoff_BusyLockReturnsRetryableConflictAndKeepsSession`, delete:

```csharp
        Assert.IsType<LockError>(exception.InnerException);
```

- [ ] **Step 2: Run the tests to verify they fail**

Run: `dotnet test`
Expected: build error in `UcpCheckoutService.cs` — `The type or namespace name 'Xapi' does not exist in the namespace 'VirtoCommerce'`.

- [ ] **Step 3: Switch the service**

In `src/VirtoCommerce.UCP.Data/Services/UcpCheckoutService.cs`, replace `using VirtoCommerce.Xapi.Core.Infrastructure;` with `using VirtoCommerce.Platform.Core.DistributedLock;` (keep usings sorted).

Add below `private const string HandoffCapability = ...;`:

```csharp
    private static readonly TimeSpan HandoffRestoreLockTimeout = TimeSpan.FromSeconds(10);
```

Change the field and constructor parameter type from `IDistributedLockService` to `IDistributedLock`:

```csharp
    private readonly IDistributedLock _distributedLock;
```

```csharp
        IDistributedLock distributedLock,
```

Replace the lock block in `RestoreHandoff`:

```csharp
        var cacheKey = GetHandoffSessionCacheKey(request.UcpSession);
        try
        {
            return await _distributedLock.ExecuteAsync(cacheKey, () => RestoreHandoffCore(cacheKey, cancellationToken));
        }
        catch (LockError exception)
        {
            // Busy does not mean consumed: the lock holder may still reject the session and leave it valid.
            throw CreateException(ModuleConstants.ErrorCodes.HandoffInProgress,
                "ucp_session is being restored by another request. Retry the request.", StatusCodes.Status409Conflict, exception);
        }
```

with:

```csharp
        var cacheKey = GetHandoffSessionCacheKey(request.UcpSession);
        await using var handle = await _distributedLock.TryAcquireAsync(cacheKey, HandoffRestoreLockTimeout, cancellationToken);
        if (handle is null)
        {
            // Busy does not mean consumed: the lock holder may still reject the session and leave it valid.
            throw CreateException(ModuleConstants.ErrorCodes.HandoffInProgress,
                "ucp_session is being restored by another request. Retry the request.", StatusCodes.Status409Conflict);
        }

        return await RestoreHandoffCore(cacheKey, cancellationToken);
```

- [ ] **Step 4: Run the tests to verify they pass**

Run: `dotnet build` then `dotnet test`
Expected: `Build succeeded`, 0 warnings; all tests pass, including `RestoreHandoff_BusyLockReturnsRetryableConflictAndKeepsSession` and `HandoffCheckout_ReturnsContinueUrlAndRestoreReadsToken`.

- [ ] **Step 5: Commit**

```bash
git add src tests
git commit -m "feat(VCST-6052): restore handoff sessions under Platform IDistributedLock"
```

---

### Task 3: README

**Files:**
- Modify: `README.md:86`

- [ ] **Step 1: Update the lock paragraph**

Replace:

```markdown
Restore is serialized per session through the XAPI `IDistributedLockService` (`VirtoCommerce.Xapi.Core.Infrastructure`), which also uses Redis when `ConnectionStrings:RedisConnectionString` is configured and an in-process lock otherwise. The store and the lock therefore always use the same backend.
```

with:

```markdown
Restore is serialized per session through the Platform `IDistributedLock` (`VirtoCommerce.Platform.Core.DistributedLock`), which also uses Redis when `ConnectionStrings:RedisConnectionString` is configured and an in-process lock otherwise. The store and the lock therefore always use the same backend. A restore that cannot take the lock within 10 seconds returns `409 handoff_in_progress`.
```

- [ ] **Step 2: Commit**

```bash
git add README.md
git commit -m "docs(VCST-6052): describe handoff restore lock"
```

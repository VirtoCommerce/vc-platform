# Distributed lock — XAPI implementation plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Run XAPI's `IDistributedLockService` and its `ResolveSynchronized*` helpers on the Platform `IDistributedLock`, keeping the GraphQL `LockError` (`ServiceAccessLocked`) contract.

**Architecture:** One extension, `AcquireForGraphQLAsync`, turns a Platform `DistributedLockTimeoutException` into `LockError`. `PlatformDistributedLockService` implements the XAPI interface on top of it and replaces the Redis, in-memory and no-lock services. New `ResolveSynchronizedAsync` overloads take `IDistributedLock` directly and flow the GraphQL cancellation token.

**Tech Stack:** .NET 10, GraphQL.NET 8.8.3, xUnit 2.9, FluentAssertions 7, Moq.

Spec: `vc-platform/docs/superpowers/specs/2026-09-24-unified-distributed-lock-design.md`.

Prerequisite: the Platform release from plan 1 (`3.1073.0`, or the first release containing VCST-6052). Check with `gh release list --repo VirtoCommerce/vc-platform --limit 5`. If it is a different version, use it everywhere this plan says `3.1073.0`.

Repository: `C:\Projects\git\VirtoCommerce\vc-module-x-api`. Build note: `TreatWarningsAsErrors` is on; XAPI uses `DiagnosticId = "VC0015"` for its own obsolete members.

Naming note: XAPI and Platform both define `IDistributedLockService`. Files that need the Platform `IDistributedLock` and the XAPI `IDistributedLockService` import the Platform type through an alias instead of the whole Platform namespace.

---

## File structure

| File | Responsibility |
|---|---|
| Modify `src/VirtoCommerce.Xapi.Core/VirtoCommerce.Xapi.Core.csproj`, `src/VirtoCommerce.Xapi.Data/VirtoCommerce.Xapi.Data.csproj`, `tests/VirtoCommerce.Xapi.Tests/VirtoCommerce.Xapi.Tests.csproj`, `src/VirtoCommerce.Xapi.Web/module.manifest` | Platform 3.1073.0 |
| Create `src/VirtoCommerce.Xapi.Core/Infrastructure/DistributedLockGraphQLExtensions.cs` | Timeout → `LockError` |
| Create `src/VirtoCommerce.Xapi.Data/Services/PlatformDistributedLockService.cs` | XAPI `IDistributedLockService` on `IDistributedLock` |
| Modify `src/VirtoCommerce.Xapi.Data/Extensions/ServiceCollectionExtensions.cs:30-43` | Registration |
| Modify `src/VirtoCommerce.Xapi.Data/Services/DistributedLockService.cs`, `InMemoryLockService.cs`, `NoLockService.cs` | `[Obsolete]` |
| Modify `src/VirtoCommerce.Xapi.Core/Infrastructure/IDistributedLockService.cs` | Deprecation remarks |
| Modify `src/VirtoCommerce.Xapi.Core/Extensions/FieldTypeExtensions.cs` | `ResolveSynchronizedAsync` overload taking `IDistributedLock` |
| Create `tests/VirtoCommerce.Xapi.Tests/Helpers/Stubs/TestDistributedLock.cs` | Test double |
| Create `tests/VirtoCommerce.Xapi.Tests/Infrastructure/DistributedLockGraphQLExtensionsTests.cs`, `tests/VirtoCommerce.Xapi.Tests/Services/PlatformDistributedLockServiceTests.cs`, `tests/VirtoCommerce.Xapi.Tests/Extensions/FieldTypeExtensionsTests.cs` | Tests |

---

### Task 1: Branch and Platform upgrade

- [ ] **Step 1: Create the branch from `dev`**

```bash
cd C:/Projects/git/VirtoCommerce/vc-module-x-api
git status --short
git fetch origin
git switch -c feat/VCST-6052 origin/dev
```

Expected: `git status --short` prints nothing before switching (commit or stash other work first).

- [ ] **Step 2: Upgrade Platform packages**

In `src/VirtoCommerce.Xapi.Core/VirtoCommerce.Xapi.Core.csproj`, add to the `PackageReference` item group (keep alphabetical order):

```xml
    <PackageReference Include="VirtoCommerce.Platform.Core" Version="3.1073.0" />
```

In `src/VirtoCommerce.Xapi.Data/VirtoCommerce.Xapi.Data.csproj`, change:

```xml
    <PackageReference Include="VirtoCommerce.Platform.Security" Version="3.1052.0" />
```

to:

```xml
    <PackageReference Include="VirtoCommerce.Platform.Security" Version="3.1073.0" />
```

In `tests/VirtoCommerce.Xapi.Tests/VirtoCommerce.Xapi.Tests.csproj`, change `VirtoCommerce.Platform.Caching` `3.1052.0` to `3.1073.0`.

In `src/VirtoCommerce.Xapi.Web/module.manifest`, change `<platformVersion>3.1052.0</platformVersion>` to `<platformVersion>3.1073.0</platformVersion>`.

- [ ] **Step 3: Build and test**

Run: `dotnet build` then `dotnet test`
Expected: `Build succeeded`; all existing tests pass. Fix any `NU1605` downgrade error by raising the named package to the version NuGet reports.

- [ ] **Step 4: Commit**

```bash
git add src tests
git commit -m "chore(VCST-6052): upgrade Platform to 3.1073.0"
```

---

### Task 2: Timeout → `LockError`

**Files:**
- Create: `src/VirtoCommerce.Xapi.Core/Infrastructure/DistributedLockGraphQLExtensions.cs`
- Create: `tests/VirtoCommerce.Xapi.Tests/Helpers/Stubs/TestDistributedLock.cs`
- Test: `tests/VirtoCommerce.Xapi.Tests/Infrastructure/DistributedLockGraphQLExtensionsTests.cs`

- [ ] **Step 1: Write the test double**

```csharp
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.DistributedLock;

namespace VirtoCommerce.Xapi.Tests.Helpers.Stubs
{
    public sealed class TestDistributedLock : IDistributedLock
    {
        public bool IsBusy { get; set; }

        public ConcurrentQueue<string> Resources { get; } = new();

        public ConcurrentQueue<CancellationToken> Tokens { get; } = new();

        public int Released => _released;

        private int _released;

        public Task<IDistributedLockHandle> AcquireAsync(string resource, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
        {
            Resources.Enqueue(resource);
            Tokens.Enqueue(cancellationToken);

            return IsBusy
                ? Task.FromException<IDistributedLockHandle>(new DistributedLockTimeoutException(resource, timeout ?? TimeSpan.FromSeconds(10)))
                : Task.FromResult<IDistributedLockHandle>(new Handle(this, resource));
        }

        public Task<IDistributedLockHandle> TryAcquireAsync(string resource, TimeSpan timeout = default, CancellationToken cancellationToken = default)
        {
            Resources.Enqueue(resource);
            Tokens.Enqueue(cancellationToken);

            return Task.FromResult<IDistributedLockHandle>(IsBusy ? null : new Handle(this, resource));
        }

        private sealed class Handle : IDistributedLockHandle
        {
            private readonly TestDistributedLock _owner;

            public Handle(TestDistributedLock owner, string resource)
            {
                _owner = owner;
                Resource = resource;
            }

            public string Resource { get; }

            public void Dispose()
            {
                Interlocked.Increment(ref _owner._released);
            }

            public ValueTask DisposeAsync()
            {
                Dispose();
                return ValueTask.CompletedTask;
            }
        }
    }
}
```

- [ ] **Step 2: Write the failing tests**

```csharp
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using VirtoCommerce.Platform.Core.DistributedLock;
using VirtoCommerce.Xapi.Core.Helpers;
using VirtoCommerce.Xapi.Core.Infrastructure;
using VirtoCommerce.Xapi.Tests.Helpers.Stubs;
using Xunit;

namespace VirtoCommerce.Xapi.Tests.Infrastructure
{
    public class DistributedLockGraphQLExtensionsTests
    {
        [Fact]
        public async Task AcquireForGraphQLAsync_WhenFree_ReturnsHandle()
        {
            var distributedLock = new TestDistributedLock();

            await using var handle = await distributedLock.AcquireForGraphQLAsync("Cart:user-1", CancellationToken.None);

            handle.Resource.Should().Be("Cart:user-1");
        }

        [Fact]
        public async Task AcquireForGraphQLAsync_WhenBusy_ThrowsLockErrorWithTimeoutAsInnerException()
        {
            var distributedLock = new TestDistributedLock { IsBusy = true };

            var act = () => distributedLock.AcquireForGraphQLAsync("Cart:user-1", CancellationToken.None);

            var error = (await act.Should().ThrowAsync<LockError>()).Which;
            error.Code.Should().Be(Constants.LockedCode);
            error.InnerException.Should().BeOfType<DistributedLockTimeoutException>();
        }
    }
}
```

- [ ] **Step 3: Run the tests to verify they fail**

Run: `dotnet test --filter "FullyQualifiedName~DistributedLockGraphQLExtensionsTests"`
Expected: build error `'TestDistributedLock' does not contain a definition for 'AcquireForGraphQLAsync'`.

- [ ] **Step 4: Implement the extension**

```csharp
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.DistributedLock;

namespace VirtoCommerce.Xapi.Core.Infrastructure
{
    public static class DistributedLockGraphQLExtensions
    {
        /// <summary>
        /// Acquires <paramref name="resource"/> with the Platform default timeout. A timeout becomes a GraphQL
        /// <see cref="LockError"/> (<c>ServiceAccessLocked</c>), so resolvers report a busy resource to the client.
        /// </summary>
        public static async Task<IDistributedLockHandle> AcquireForGraphQLAsync(this IDistributedLock distributedLock, string resource, CancellationToken cancellationToken)
        {
            try
            {
                return await distributedLock.AcquireAsync(resource, cancellationToken: cancellationToken);
            }
            catch (DistributedLockTimeoutException exception)
            {
                throw new LockError("Service is busy.", exception);
            }
        }
    }
}
```

- [ ] **Step 5: Run the tests to verify they pass**

Run: `dotnet test --filter "FullyQualifiedName~DistributedLockGraphQLExtensionsTests"`
Expected: PASS (2 tests).

- [ ] **Step 6: Commit**

```bash
git add src/VirtoCommerce.Xapi.Core/Infrastructure/DistributedLockGraphQLExtensions.cs tests/VirtoCommerce.Xapi.Tests
git commit -m "feat(VCST-6052): map Platform lock timeouts to GraphQL LockError"
```

---

### Task 3: XAPI `IDistributedLockService` on Platform

**Files:**
- Create: `src/VirtoCommerce.Xapi.Data/Services/PlatformDistributedLockService.cs`
- Modify: `src/VirtoCommerce.Xapi.Data/Extensions/ServiceCollectionExtensions.cs:30-43`
- Modify: `src/VirtoCommerce.Xapi.Data/Services/DistributedLockService.cs`, `InMemoryLockService.cs`, `NoLockService.cs`
- Modify: `src/VirtoCommerce.Xapi.Core/Infrastructure/IDistributedLockService.cs`
- Test: `tests/VirtoCommerce.Xapi.Tests/Services/PlatformDistributedLockServiceTests.cs`

- [ ] **Step 1: Write the failing tests**

```csharp
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Xapi.Core.Infrastructure;
using VirtoCommerce.Xapi.Data.Extensions;
using VirtoCommerce.Xapi.Data.Services;
using VirtoCommerce.Xapi.Tests.Helpers.Stubs;
using Xunit;
using IDistributedLock = VirtoCommerce.Platform.Core.DistributedLock.IDistributedLock;

namespace VirtoCommerce.Xapi.Tests.Services
{
    public class PlatformDistributedLockServiceTests
    {
        [Fact]
        public async Task ExecuteAsync_RunsResolverUnderLockAndReleases()
        {
            var distributedLock = new TestDistributedLock();
            var service = new PlatformDistributedLockService(distributedLock);

            var result = await service.ExecuteAsync("Cart:user-1", () => Task.FromResult(5));

            result.Should().Be(5);
            distributedLock.Resources.Should().ContainSingle().Which.Should().Be("Cart:user-1");
            distributedLock.Released.Should().Be(1);
        }

        [Fact]
        public void Execute_RunsResolverUnderLockAndReleases()
        {
            var distributedLock = new TestDistributedLock();
            var service = new PlatformDistributedLockService(distributedLock);

            service.Execute("Cart:user-1", () => 6).Should().Be(6);
            distributedLock.Released.Should().Be(1);
        }

        [Fact]
        public async Task ExecuteAsync_WhenBusy_ThrowsLockError()
        {
            var service = new PlatformDistributedLockService(new TestDistributedLock { IsBusy = true });

            var act = () => service.ExecuteAsync("Cart:user-1", () => Task.FromResult(5));

            await act.Should().ThrowAsync<LockError>();
        }

        [Fact]
        public void AddDistributedLockService_RegistersPlatformAdapter()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IDistributedLock>(new TestDistributedLock());

            services.AddDistributedLockService(new ConfigurationBuilder().Build());

            using var provider = services.BuildServiceProvider();
            provider.GetRequiredService<IDistributedLockService>().Should().BeOfType<PlatformDistributedLockService>();
        }
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

Run: `dotnet test --filter "FullyQualifiedName~PlatformDistributedLockServiceTests"`
Expected: build error `The type or namespace name 'PlatformDistributedLockService' could not be found`.

- [ ] **Step 3: Implement the adapter**

```csharp
using System;
using System.Threading;
using System.Threading.Tasks;
using VirtoCommerce.Xapi.Core.Infrastructure;
using IDistributedLock = VirtoCommerce.Platform.Core.DistributedLock.IDistributedLock;

namespace VirtoCommerce.Xapi.Data.Services
{
    /// <summary>
    /// XAPI <see cref="IDistributedLockService"/> on the Platform <see cref="IDistributedLock"/>:
    /// Redis when configured, otherwise an in-process lock; waits <c>DistributedLock:DefaultTimeout</c>
    /// and throws <see cref="LockError"/> when the resource stays busy.
    /// </summary>
    public class PlatformDistributedLockService : IDistributedLockService
    {
        private readonly IDistributedLock _distributedLock;

        public PlatformDistributedLockService(IDistributedLock distributedLock)
        {
            _distributedLock = distributedLock;
        }

        public virtual T Execute<T>(string resourceKey, Func<T> resolver)
        {
            // Synchronous legacy entry point; IDistributedLock is async only.
            using var handle = _distributedLock.AcquireForGraphQLAsync(resourceKey, CancellationToken.None).GetAwaiter().GetResult();

            return resolver();
        }

        public virtual async Task<T> ExecuteAsync<T>(string resourceKey, Func<Task<T>> resolver)
        {
            await using var handle = await _distributedLock.AcquireForGraphQLAsync(resourceKey, CancellationToken.None);

            return await resolver();
        }
    }
}
```

- [ ] **Step 4: Replace the registration**

In `src/VirtoCommerce.Xapi.Data/Extensions/ServiceCollectionExtensions.cs`, replace the body of `AddDistributedLockService`:

```csharp
        public static IServiceCollection AddDistributedLockService(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("RedisConnectionString");
            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                services.AddSingleton<IDistributedLockService, DistributedLockService>();
            }
            else
            {
                services.AddSingleton<IDistributedLockService, InMemoryLockService>();
            }

            return services;
        }
```

with:

```csharp
        /// <summary>
        /// Registers the XAPI <see cref="IDistributedLockService"/> on the Platform <c>IDistributedLock</c>,
        /// which selects Redis or the in-process lock from <c>ConnectionStrings:RedisConnectionString</c>.
        /// </summary>
        public static IServiceCollection AddDistributedLockService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDistributedLockService, PlatformDistributedLockService>();

            return services;
        }
```

- [ ] **Step 5: Mark the replaced services obsolete**

Add above the class declaration in `src/VirtoCommerce.Xapi.Data/Services/DistributedLockService.cs`, `InMemoryLockService.cs` and `NoLockService.cs` (`using System;` is already present):

```csharp
    [Obsolete("No longer registered. XAPI uses PlatformDistributedLockService on the Platform IDistributedLock.", DiagnosticId = "VC0015", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
```

- [ ] **Step 6: Add the deprecation remarks**

Replace `src/VirtoCommerce.Xapi.Core/Infrastructure/IDistributedLockService.cs` with:

```csharp
using System;
using System.Threading.Tasks;

namespace VirtoCommerce.Xapi.Core.Infrastructure
{
    /// <summary>
    /// Runs a resolver under a distributed lock and reports a busy resource as <see cref="LockError"/>.
    /// </summary>
    /// <remarks>
    /// Deprecated: resolve <c>VirtoCommerce.Platform.Core.DistributedLock.IDistributedLock</c> and use
    /// <see cref="DistributedLockGraphQLExtensions.AcquireForGraphQLAsync"/>, or the
    /// <c>ResolveSynchronizedAsync</c> overload that takes <c>IDistributedLock</c>.
    /// </remarks>
    public interface IDistributedLockService
    {
        T Execute<T>(string resourceKey, Func<T> resolver);

        Task<T> ExecuteAsync<T>(string resourceKey, Func<Task<T>> resolver);
    }
}
```

- [ ] **Step 7: Build and run the tests**

Run: `dotnet build` then `dotnet test --filter "FullyQualifiedName~PlatformDistributedLockServiceTests"`
Expected: `Build succeeded`; PASS (4 tests). If the build reports `VC0015` where an obsolete service is still referenced, that reference is dead code from the old registration; delete it.

- [ ] **Step 8: Commit**

```bash
git add src tests
git commit -m "feat(VCST-6052): run XAPI IDistributedLockService on Platform IDistributedLock"
```

---

### Task 4: `ResolveSynchronizedAsync` on `IDistributedLock`

**Files:**
- Modify: `src/VirtoCommerce.Xapi.Core/Extensions/FieldTypeExtensions.cs`
- Test: `tests/VirtoCommerce.Xapi.Tests/Extensions/FieldTypeExtensionsTests.cs`

- [ ] **Step 1: Write the failing tests**

```csharp
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GraphQL;
using GraphQL.Builders;
using GraphQL.Execution;
using GraphQL.Types;
using VirtoCommerce.Xapi.Core.Extensions;
using VirtoCommerce.Xapi.Core.Infrastructure;
using VirtoCommerce.Xapi.Tests.Helpers.Stubs;
using Xunit;

namespace VirtoCommerce.Xapi.Tests.Extensions
{
    public class FieldTypeExtensionsTests
    {
        [Fact]
        public async Task ResolveSynchronizedAsync_WithDistributedLock_LocksPrefixedKeyAndFlowsCancellation()
        {
            var distributedLock = new TestDistributedLock();
            using var cancellation = new CancellationTokenSource();
            var field = FieldBuilder<object, int>.Create("field", typeof(IntGraphType))
                .ResolveSynchronizedAsync("Cart", "userId", distributedLock, _ => Task.FromResult(9));

            var result = await field.FieldType.Resolver!.ResolveAsync(CreateContext("user-1", cancellation.Token));

            result.Should().Be(9);
            distributedLock.Resources.Should().ContainSingle().Which.Should().Be("Cart:user-1");
            distributedLock.Tokens.Should().ContainSingle().Which.Should().Be(cancellation.Token);
            distributedLock.Released.Should().Be(1);
        }

        [Fact]
        public async Task ResolveSynchronizedAsync_WithoutResourceKey_ResolvesWithoutLock()
        {
            var distributedLock = new TestDistributedLock();
            var field = FieldBuilder<object, int>.Create("field", typeof(IntGraphType))
                .ResolveSynchronizedAsync("Cart", "userId", distributedLock, _ => Task.FromResult(9));

            var result = await field.FieldType.Resolver!.ResolveAsync(CreateContext(userId: null, CancellationToken.None));

            result.Should().Be(9);
            distributedLock.Resources.Should().BeEmpty();
        }

        [Fact]
        public async Task ResolveSynchronizedAsync_WhenBusy_ThrowsLockError()
        {
            var distributedLock = new TestDistributedLock { IsBusy = true };
            var field = FieldBuilder<object, int>.Create("field", typeof(IntGraphType))
                .ResolveSynchronizedAsync("Cart", "userId", distributedLock, _ => Task.FromResult(9));

            var act = async () => await field.FieldType.Resolver!.ResolveAsync(CreateContext("user-1", CancellationToken.None));

            await act.Should().ThrowAsync<LockError>();
        }

        private static ResolveFieldContext<object> CreateContext(string userId, CancellationToken cancellationToken)
        {
            var command = new Dictionary<string, object>();
            if (userId != null)
            {
                command["userId"] = userId;
            }

            return new ResolveFieldContext<object>
            {
                Arguments = new Dictionary<string, ArgumentValue>
                {
                    ["command"] = new ArgumentValue(command, ArgumentSource.Literal),
                },
                CancellationToken = cancellationToken,
            };
        }
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

Run: `dotnet test --filter "FullyQualifiedName~FieldTypeExtensionsTests"`
Expected: build error — no `ResolveSynchronizedAsync` overload accepts `TestDistributedLock`.

- [ ] **Step 3: Add the overload**

In `src/VirtoCommerce.Xapi.Core/Extensions/FieldTypeExtensions.cs`, add the alias below the existing usings:

```csharp
using IDistributedLock = VirtoCommerce.Platform.Core.DistributedLock.IDistributedLock;
```

Add after the existing `ResolveSynchronizedAsync` method:

```csharp
        /// <summary>
        /// Resolves the field under the Platform distributed lock <c>{resourceKeyPrefix}:{command[resourceKeyProperty]}</c>.
        /// Resolves without a lock when the command has no such property. A busy resource becomes <see cref="LockError"/>.
        /// </summary>
        public static FieldBuilder<TSourceType, TReturnType> ResolveSynchronizedAsync<TSourceType, TReturnType>(
            this FieldBuilder<TSourceType, TReturnType> fieldBuilder,
            string resourceKeyPrefix,
            string resourceKeyProperty,
            IDistributedLock distributedLock,
            Func<IResolveFieldContext<TSourceType>, Task<TReturnType>> resolve)
        {
            fieldBuilder.FieldType.Resolver = new FuncFieldResolver<TSourceType, TReturnType>(ctx => ResolveWrapperAsync(ctx));

            return fieldBuilder;

            async ValueTask<TReturnType> ResolveWrapperAsync(IResolveFieldContext<TSourceType> context)
            {
                var resourceKey = GetResourceKey(context, resourceKeyPrefix, resourceKeyProperty);
                if (string.IsNullOrEmpty(resourceKey))
                {
                    return await resolve(context);
                }

                await using var handle = await distributedLock.AcquireForGraphQLAsync(resourceKey, context.CancellationToken);
                return await resolve(context);
            }
        }
```

- [ ] **Step 4: Run the tests to verify they pass**

Run: `dotnet test --filter "FullyQualifiedName~FieldTypeExtensionsTests"`
Expected: PASS (3 tests).

- [ ] **Step 5: Commit**

```bash
git add src/VirtoCommerce.Xapi.Core/Extensions/FieldTypeExtensions.cs tests/VirtoCommerce.Xapi.Tests/Extensions/FieldTypeExtensionsTests.cs
git commit -m "feat(VCST-6052): add ResolveSynchronizedAsync overload on Platform IDistributedLock"
```

---

### Task 5: Full verification

- [ ] **Step 1: Build and test**

Run: `dotnet build` then `dotnet test`
Expected: `Build succeeded`, 0 warnings; all tests pass (9 new).

- [ ] **Step 2: Check x-cart still compiles against the new package**

x-cart calls `ResolveSynchronizedAsync(CartPrefix, "userId", _distributedLockService, ...)` with the XAPI interface, so it keeps binding to the existing overload. No change is required in x-cart for this release.

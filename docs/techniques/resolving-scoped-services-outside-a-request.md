# Resolving scoped services outside a request

Singletons, event handlers and background jobs have no request scope, so a scoped service such as a `DbContext`-backed repository or `UserManager<ApplicationUser>` cannot be injected into them directly. Such a service has to be created in a DI scope of its own, and that scope has to be released when the work is done.

## `IScopedServiceFactory<T>`

Inject `IScopedServiceFactory<T>` for the service you need. The platform registers it once as an open generic, so no per-type registration is needed in your module:

```csharp
public class MyService(IScopedServiceFactory<ICatalogRepository> repositoryFactory)
{
    public async Task DoWork()
    {
        using var scoped = repositoryFactory.Create();
        var repository = scoped.Service;
        ...
    }
}
```

`Create()` returns a `ScopedService<T>`. Its `Service` property is the resolved instance; disposing the wrapper disposes the scope, and with it the service and everything else the scope created. `await using` works as well. `ScopedService<T>.ServiceProvider` resolves further services from the same scope when two services must share it, for example a `UserManager<ApplicationUser>` and the `RoleManager<Role>` it works with.

The same operation is available from startup code through `IServiceScopeFactory.CreateScopedService<T>()` and `IServiceProvider.CreateScopedService<T>()`.

Unit tests of a consumer can hand it a wrapper created without a scope. Such a wrapper owns the instance itself: disposing it disposes the instance when it is disposable, and nothing else.

```csharp
var factory = new Mock<IScopedServiceFactory<ICatalogRepository>>();
factory.Setup(x => x.Create()).Returns(() => new ScopedService<ICatalogRepository>(repositoryMock.Object));
```

## Compatibility with the `Func<T>` signatures

Platform services that used to take `Func<IPlatformRepository>`, `Func<ISecurityRepository>`, `Func<UserManager<ApplicationUser>>` or `Func<RoleManager<Role>>` keep those constructors, marked `[Obsolete]`. They delegate to the new constructor through `DelegateScopedServiceFactory<T>`, which turns a `Func<T>` into an `IScopedServiceFactory<T>`:

```csharp
IScopedServiceFactory<ICatalogRepository> factory = new DelegateScopedServiceFactory<ICatalogRepository>(() => repositoryMock.Object);
```

A derived class that still calls the old `base(...)` constructor compiles with a deprecation warning and behaves as before. `CustomPasswordValidator` keeps its protected `_repositoryFactory` field for the same reason; new code uses `_scopedRepositoryFactory`.

A class that has both constructors cannot be registered with a plain `AddSingleton<TService, TImplementation>()`: the container sees two resolvable constructors with different parameter types and refuses to activate the type. The platform registers such classes with `services.AddActivated<TService, TImplementation>(ServiceLifetime.Singleton)` (or `TryAddActivated`), which selects the constructor marked `[ActivatorUtilitiesConstructor]`. Use the same helper if your own class keeps a legacy constructor alongside the new one.

## Legacy `Func<T>` factories

Older code injects `Func<ICatalogRepository>` or `Func<UserManager<ApplicationUser>>` and calls `using var repository = _repositoryFactory();`. Those factories are written as `provider.CreateScope().ServiceProvider.GetService<T>()`: nothing owns the scope they create, so the scope and every service in it stay alive until the returned instance is garbage collected, and a misconfigured `T` comes back as `null` instead of failing at the call. The registrations are kept for compatibility. Move a consumer to `IScopedServiceFactory<T>` when you touch it, and do not register new factories in that shape.

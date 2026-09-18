# Resolving scoped services outside a request

Singletons, event handlers and background jobs have no request scope, so a scoped service such as a `DbContext`-backed repository or `UserManager<ApplicationUser>` cannot be injected into them directly. The platform offers two ways to obtain one.

## `Func<T>` factories

Every module registers a factory for its repository, and the platform registers factories for the Identity managers:

```csharp
serviceCollection.AddTransient<Func<ICatalogRepository>>(provider => () => provider.ResolveInOwnScope<ICatalogRepository>());
```

`ResolveInOwnScope<T>()` creates a DI scope, resolves `T` from it and, when `T` implements `IServiceScopeOwner`, hands the scope over to the service. Disposing the service then disposes the scope and everything the scope created. `DbContextRepositoryBase<TContext>`, `CustomUserManager` and `CustomRoleManager` implement `IServiceScopeOwner`, so the usual consumer code releases everything deterministically:

```csharp
using var repository = _repositoryFactory();
using var userManager = _userManagerFactory();
```

Do not write a factory as `provider.CreateScope().ServiceProvider.GetService<T>()`. Nothing owns that scope, so the scope and every service it created stay alive until the garbage collector runs, and a misconfigured `T` comes back as `null` instead of failing at the call.

## `IServiceScopeFactory`

When `T` cannot own a scope, for example `SignInManager<ApplicationUser>`, an aggregate, or any service that is not `IDisposable`, own the scope yourself:

```csharp
using var scope = _serviceScopeFactory.CreateScope();
var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<ApplicationUser>>();
```

## Implementing `IServiceScopeOwner`

A disposable class that is produced through `ResolveInOwnScope<T>()` can take ownership of its scope: store the scope passed to `OwnScope` and dispose it at the end of `Dispose(bool)`. Disposing the scope disposes the owner once more, so `Dispose(bool)` must tolerate a second call.

```csharp
public class MyRepository : DbContextRepositoryBase<MyDbContext>
{
    // Nothing to do: the base class already owns and disposes the scope.
}

public class MyUserManager : CustomUserManager
{
    // Nothing to do either: CustomUserManager owns and disposes the scope.
}
```

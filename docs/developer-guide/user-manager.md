# Working with User Manager

VirtoCommerce security system is fully based on ASP.Net Identity framework. All security services are derived from Identity classes and extend its functionality.<br><br>Identity User Manager is a class used for managment security accounts with no direct access to the security DB context. In VirtoCommerce, it has a custom implementation, **VirtoCommerce.Platform.Web.Security.CustomUserManager**. Role Manager has a custom implementation as well: **VirtoCommerce.Platform.Web.Security.CustomRoleManager**.

## Injection
The custom User Manager is registered in the DI as the **UserManager<ApplicationUser>** type. To prevent multithread proplems, you should use a factory to get User Manager:

```csharp
services.TryAddScoped<UserManager<ApplicationUser>, CustomUserManager>();
```

The platform registers `IScopedServiceFactory<T>` once as an open generic, so `IScopedServiceFactory<UserManager<ApplicationUser>>` can be injected anywhere without further registration. Each `Create()` call resolves the manager in a DI scope of its own and returns a `ScopedService<T>`; disposing that wrapper disposes the scope, and with it the manager and its security `DbContext`. See [Resolving scoped services outside a request](../techniques/resolving-scoped-services-outside-a-request.md) for the general pattern.

The older `Func<UserManager<ApplicationUser>>` registration is still present for compatibility, but it does not release the scope it creates.

You can create a specific User Manager implementation by creating a class inherited from **CustomUserManager** or **AspNetUserManager<ApplicationUser>** directly and registering it in the DI in your own module:

```csharp
public void Initialize(IServiceCollection serviceCollection) 
{
  ...
  serviceCollection.AddScoped<UserManager<ApplicationUser>, MyCustomUserManager>();
  ...
}
```

**CustomRoleManager** is registered in the same way.

## Usage
You can get both user and role managers by adding `IScopedServiceFactory<T>` to your service constructor. Each `Create()` call resolves the manager in a DI scope of its own; disposing the returned `ScopedService<T>` releases the manager together with its security `DbContext`:

```csharp
    public class MyCoolService 
    {
        private readonly IScopedServiceFactory<UserManager<ApplicationUser>> _userManagerFactory;
        private readonly IScopedServiceFactory<RoleManager<Role>> _roleManagerFactory;
    
        public MyCoolService(IScopedServiceFactory<UserManager<ApplicationUser>> userManagerFactory, IScopedServiceFactory<RoleManager<Role>> roleManagerFactory)
        {
            _userManagerFactory = userManagerFactory;
            _roleManagerFactory = roleManagerFactory;
        }
        
        public void DoMyCoolWork()
        {
            using var userManager = _userManagerFactory.Create();
            using var roleManager = _roleManagerFactory.Create();
            userManager.Service.FindByNameAsync(...);
            ...
        }
    }
```

The `Func<UserManager<ApplicationUser>>` and `Func<RoleManager<Role>>` factories shown above are kept for compatibility. They do not release the scope they create, so prefer `IScopedServiceFactory<T>` in new code. See [Resolving scoped services outside a request](../techniques/resolving-scoped-services-outside-a-request.md).

## Recomendations

In common cases, you do not need to get user or role manager directly by type. Use factories to create a manager just before an operation.

## References

Check out these articles for more information on user and role management:

+ https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.usermanager-1?view=aspnetcore-6.0
+ https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.rolemanager-1?view=aspnetcore-6.0
+ https://codewithmukesh.com/blog/user-management-in-aspnet-core-mvc/

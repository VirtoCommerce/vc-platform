# Working with User Manager

VirtoCommerce security system is fully based on ASP.Net Identity framework. All security services are derived from Identity classes and extend its functionality.<br><br>Identity User Manager is a class used for managment security accounts with no direct access to the security DB context. In VirtoCommerce, it has a custom implementation, **VirtoCommerce.Platform.Web.Security.CustomUserManager**. Role Manager has a custom implementation as well: **VirtoCommerce.Platform.Web.Security.CustomRoleManager**.

## Injection
The custom User Manager is registered in the DI as the **UserManager<ApplicationUser>** type. To prevent multithread proplems, you should use a factory to get User Manager:

```csharp
services.TryAddScoped<UserManager<ApplicationUser>, CustomUserManager>();
services.AddSingleton<Func<UserManager<ApplicationUser>>>(provider => () => provider.CreateScope().ServiceProvider.GetService<UserManager<ApplicationUser>>());
```

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
You can get both user and role managers by adding a respective factory to your service constructor:

```csharp
    public class MyCoolService 
    {
        private readonly Func<UserManager<ApplicationUser>> _userManagerFactory;
        private readonly Func<RoleManager<Role>> _roleManagerFactory;
    
        public MyCoolService(Func<UserManager<ApplicationUser>> userManagerFactory, Func<RoleManager<Role>> roleManagerFactory)
        {
            _userManagerFactory = userManagerFactory;
            _roleManagerFactory = roleManagerFactory;
        }
        
        public void DoMyCoolWork()
        {
            using var userManager = userManagerFactory();
            using var roleManager = roleManagerFactory();
            ...
        }
    }
```

## Recomendations

In common cases, you do not need to get user or role manager directly by type. Use factories to create a manager just before an operation.

## References

Check out these articles for more information on user and role management:

+ https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.usermanager-1?view=aspnetcore-6.0
+ https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.rolemanager-1?view=aspnetcore-6.0
+ https://codewithmukesh.com/blog/user-management-in-aspnet-core-mvc/

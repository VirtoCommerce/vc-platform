# Working with User Manager

Identity User Manager is a class used for managment security accounts with no direct access to the security DB context. In VirtoCommerce it has a custom implementation **VirtoCommerce.Platform.Web.Security.CustomUserManager**. Role Manager has a custom implementation as well: **VirtoCommerce.Platform.Web.Security.CustomRoleManager**

## Injection
The custom user manager is regestered in the DI as **UserManager<ApplicationUser>** type. To pervent multithread proplems a factory used to get the UserManager:
```csharp
services.TryAddScoped<UserManager<ApplicationUser>, CustomUserManager>();
services.AddSingleton<Func<UserManager<ApplicationUser>>>(provider => () => provider.CreateScope().ServiceProvider.GetService<UserManager<ApplicationUser>>());
```
  
You can create a specific user manager implementation by creating a class inherited from the **CustomUserManager** or **AspNetUserManager<ApplicationUser>** directly and registering it in the DI in your own module:
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
You can get both user and role managers by adding corresponding factory to your service's constructor:
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

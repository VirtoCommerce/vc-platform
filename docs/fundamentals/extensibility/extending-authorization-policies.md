
VirtoCommerce supports extension of existing authorization policies that are defined and checked in the Api controllers and other places. This article shows how to use the various techniques to extend exist authorization policies type without direct code modification.


[View or download sample code](https://github.com/VirtoCommerce/vc-module-order/tree/dev/samples/VirtoCommerce.OrdersModule2.Web/Authorization)

## Extending existing authorization policies

Let's say we have this authorization checks in the OrderModule. And we want to extend the default `OrderAuthorizationHandler` is associated with this requirement `OrderAuthorizationRequirement` that is called during this authorization check with a new policy  that will limit the resulting orders by their statuses. To be able create a role that allows for concrete users see orders only with specific state(s).


You can read more about how the authorization policies work by this link [Policy-based authorization in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-5.0)


*`OrderModuleController.cs`*
```C#
[HttpPost]
[Route("api/order/customerOrders/search")]
 public async Task<ActionResult<CustomerOrderSearchResult>> SearchCustomerOrder([FromBody] CustomerOrderSearchCriteria criteria)
        {
            var authorizationResult = await _authorizationService.AuthorizeAsync(User, criteria, new OrderAuthorizationRequirement(ModuleConstants.Security.Permissions.Read));
            if (!authorizationResult.Succeeded)
            {
                return Unauthorized();
            }
        }

```
In order to make this extension we need to define a new `CustomOrderAuthorizationHandler` class and use the same requirement `OrderAuthorizationRequirement` as it used in the original controller method for authorization check.


*`CustomOrderAuthorizationHandler.cs`*
```C#
 public sealed class CustomOrderAuthorizationHandler : PermissionAuthorizationHandlerBase<OrderAuthorizationRequirement>
    {
        //Code skipped for better clarity
    }
```
The next step you need to register your handler in the DI to tell for ASP.NET Authorization to call your handler along with another that are associated with `OrderAuthorizationRequirement` requirement.

*`Module.cs`*
```C#
 public class Module : IModule
    {
        public void Initialize(IServiceCollection serviceCollection)
        {
            //Rest of code skipped for better clarity 
            serviceCollection.AddTransient<IAuthorizationHandler, CustomOrderAuthorizationHandler>();
        
        }
    }
```
After this point the custom `CustomOrderAuthorizationHandler`  along with another registered handlers will be executed each time when  `OrderAuthorizationRequirement`  being checked by this call  

```C#
IAuthorizationService.AuthorizeAsync(User, data, new OrderAuthorizationRequirement());
```

## Additional resources

* [Make secure Web API](https://github.com/VirtoCommerce/vc-platform/blob/master/docs/fundamentals/make-secure-webapi.md)
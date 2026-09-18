using System;
using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Core.Common;

public static class ServiceProviderExtensions
{
    // Resolves T from a new DI scope. When T implements IServiceScopeOwner the scope is disposed together with the service;
    // otherwise the scope lives as long as the service instance. Prefer IServiceScopeFactory for services that cannot own a scope.
    public static T ResolveInOwnScope<T>(this IServiceProvider provider) where T : class
    {
        var scope = provider.CreateScope();

        try
        {
            var service = scope.ServiceProvider.GetRequiredService<T>();

            if (service is IServiceScopeOwner owner)
            {
                owner.OwnScope(scope);
            }

            return service;
        }
        catch
        {
            scope.Dispose();
            throw;
        }
    }
}

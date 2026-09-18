using System;
using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Core.Common;

public static class ServiceScopeFactoryExtensions
{
    public static ScopedService<T> CreateScopedService<T>(this IServiceScopeFactory scopeFactory)
        where T : class
    {
        var scope = scopeFactory.CreateScope();

        try
        {
            return new ScopedService<T>(scope.ServiceProvider.GetRequiredService<T>(), scope);
        }
        catch
        {
            scope.Dispose();
            throw;
        }
    }

    public static ScopedService<T> CreateScopedService<T>(this IServiceProvider provider)
        where T : class
    {
        return provider.GetRequiredService<IServiceScopeFactory>().CreateScopedService<T>();
    }
}

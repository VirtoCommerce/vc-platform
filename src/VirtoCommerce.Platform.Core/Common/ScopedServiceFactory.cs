using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Core.Common;

public class ScopedServiceFactory<T>(IServiceScopeFactory scopeFactory) : IScopedServiceFactory<T>
    where T : class
{
    public ScopedService<T> Create()
    {
        return scopeFactory.CreateScopedService<T>();
    }
}

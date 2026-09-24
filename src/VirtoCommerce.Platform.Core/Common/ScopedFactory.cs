using Microsoft.Extensions.DependencyInjection;

namespace VirtoCommerce.Platform.Core.Common;

public class ScopedFactory<T>(IServiceScopeFactory scopeFactory) : IScopedFactory<T>
    where T : class
{
    public ScopedService<T> Create()
    {
        return scopeFactory.CreateScopedService<T>();
    }
}

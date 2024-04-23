using System;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Modules;

public class OptionalDependencyManager<T> : IOptionalDependency<T>
{
    private readonly IServiceProvider _serviceProvider;

    // Idea for implematation:
    // https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Options/src/UnnamedOptionsManager.cs#L9
    public OptionalDependencyManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public bool HasValue
    {
        get
        {
            return Value != null;
        }
    }

    public T Value
    {
        get
        {
            var service = _serviceProvider.GetService<T>();
            return service;
        }
    }
}

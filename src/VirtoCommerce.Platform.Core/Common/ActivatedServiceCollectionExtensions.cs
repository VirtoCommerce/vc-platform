using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace VirtoCommerce.Platform.Core.Common;

// The default container refuses to activate a type whose constructors are all resolvable but differ in parameter types
// ("The following constructors are ambiguous"), and it ignores [ActivatorUtilitiesConstructor]. ActivatorUtilities honors it,
// so types that keep [Obsolete] legacy constructors next to their current one are registered through this factory.
public static class ActivatedServiceCollectionExtensions
{
    public static IServiceCollection AddActivated<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        services.Add(CreateDescriptor<TService, TImplementation>(lifetime));
        return services;
    }

    public static IServiceCollection TryAddActivated<TService, TImplementation>(this IServiceCollection services, ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        services.TryAdd(CreateDescriptor<TService, TImplementation>(lifetime));
        return services;
    }

    private static ServiceDescriptor CreateDescriptor<TService, TImplementation>(ServiceLifetime lifetime)
        where TService : class
        where TImplementation : class, TService
    {
        var objectFactory = ActivatorUtilities.CreateFactory(typeof(TImplementation), Type.EmptyTypes);

        return new ServiceDescriptor(typeof(TService), provider => objectFactory(provider, arguments: null), lifetime);
    }
}

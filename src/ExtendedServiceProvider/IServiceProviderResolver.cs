using Microsoft.Extensions.DependencyInjection;

namespace ExtendedServiceProvider
{
    public interface IServiceProviderResolver
    {
        object? Resolve(IKeyedServiceProvider serviceProvider, Type serviceType, object? serviceKey);
    }
}

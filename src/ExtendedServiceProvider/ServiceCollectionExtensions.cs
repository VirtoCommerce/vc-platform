using Microsoft.Extensions.DependencyInjection;

namespace ExtendedServiceProvider
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceProvider BuildExtendedServiceProvider(this IServiceCollection services, ServiceProviderOptions? options = null, IServiceProviderResolver? resolver = null)
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));
            return new ExtendedServiceProvider(services, options, resolver);
        }

        public static IServiceCollection AddServiceProviderHook<THook>(this IServiceCollection services) where THook : class, IServiceProviderHook
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));
            return services.AddSingleton<IServiceProviderHook, THook>();
        }

        public static IServiceCollection AddServiceProviderResolver<TResolver>(this IServiceCollection services) where TResolver : class, IServiceProviderResolver
        {
            ArgumentNullException.ThrowIfNull(services, nameof(services));
            return services.AddSingleton<IServiceProviderResolver, TResolver>();
        }
    }
}

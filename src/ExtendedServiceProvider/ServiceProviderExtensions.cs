using Microsoft.Extensions.DependencyInjection;

namespace ExtendedServiceProvider
{
    public static class ServiceProviderExtensions
    {
        public static Lazy<T> GetLazyService<T>(this IServiceProvider serviceProvider) where T : class
        {
            return new GenericLazy<T>(() => serviceProvider.GetService<T>()!);
        }

        public static Lazy<T> GetLazyKeyedService<T>(this IServiceProvider serviceProvider, object serviceKey) where T : class
        {
            return new GenericLazy<T>(() => serviceProvider.GetKeyedService<T>(serviceKey)!);
        }

        public static Lazy<T> GetLazyRequiredService<T>(this IServiceProvider serviceProvider) where T : class
        {
            return new GenericLazy<T>(() => serviceProvider.GetRequiredService<T>());
        }

        public static Lazy<T> GetLazyRequiredKeyedService<T>(this IServiceProvider serviceProvider, object serviceKey) where T : class
        {
            return new GenericLazy<T>(() => serviceProvider.GetRequiredKeyedService<T>(serviceKey));
        }
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExtendedServiceProvider
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseExtendedServiceProvider(this IHostBuilder builder, ServiceProviderOptions? options = null, IServiceProviderResolver? resolver = null)
        {
            ArgumentNullException.ThrowIfNull(builder, nameof(builder));
            var serviceProviderFactory = new ExtendedServiceProviderFactory(options ?? ExtendedServiceProviderFactory.DefaultServiceProviderOptions, resolver);
            return builder
                .UseServiceProviderFactory(serviceProviderFactory)
                .ConfigureServices((ctx, services) =>
                {
                    services.AddSingleton<IStartupFilter>(sp =>
                    {
                        var serviceProvider = serviceProviderFactory.CreateServiceProvider(services);
                        return new ExtendedServiceProvidersFeatureFilter(serviceProvider);
                    });
                });
        }
    }
}

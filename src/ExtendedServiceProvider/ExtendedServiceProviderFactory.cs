using Microsoft.Extensions.DependencyInjection;

namespace ExtendedServiceProvider
{
    internal class ExtendedServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
    {
        internal static readonly ServiceProviderOptions DefaultServiceProviderOptions = new() { ValidateOnBuild = true, ValidateScopes = true };

        private IServiceProvider? _serviceProvider;
        private readonly IServiceProviderResolver? _resolver;
        private readonly ServiceProviderOptions? _options;

        public ExtendedServiceProviderFactory(ServiceProviderOptions? options = null, IServiceProviderResolver? resolver = null)
        {
            _options = options;
            _resolver = resolver;
        }

        public IServiceCollection CreateBuilder(IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);
            return services;
        }

        public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
        {
            ArgumentNullException.ThrowIfNull(containerBuilder);
            _serviceProvider ??= new ExtendedServiceProvider(containerBuilder, _options, _resolver);
            return _serviceProvider;
        }
    }
}

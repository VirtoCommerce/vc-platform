using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExtendedServiceProvider
{
    public interface IServiceProviderHook
    {
        void ServiceResolved(Type serviceType, object service);
    }

    public sealed class LoggerServiceProviderHook : IServiceProviderHook
    {
        private readonly ILogger<LoggerServiceProviderHook> _logger;

        public LoggerServiceProviderHook(ILogger<LoggerServiceProviderHook> logger)
        {
            _logger = logger;
        }

        public void ServiceResolved(Type serviceType, object service)
        {
            _logger.LogDebug("ServiceResolved: serviceType = {serviceType}, service = {service}", serviceType, service);
        }
    }

    public sealed class PropertyInitializerServiceProviderHook : IServiceProviderHook
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PropertyInitializerServiceProviderHook> _logger;

        public PropertyInitializerServiceProviderHook(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetRequiredService<ILogger<PropertyInitializerServiceProviderHook>>();
        }

        public void ServiceResolved(Type serviceType, object service)
        {
            foreach (var prop in service.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.GetProperty))
            {
                var injectPropertyAttribute = prop.GetCustomAttribute<InjectAttribute>();

                if (injectPropertyAttribute != null && prop.GetValue(service) == null)
                {
                    _logger.LogDebug("ServiceResolved: serviceType = {serviceType}, setting {propertyName}", serviceType, prop.Name);
                    var propertyService = _serviceProvider.GetRequiredKeyedService(prop.PropertyType, injectPropertyAttribute.ServiceKey);
                    prop.SetValue(service, propertyService);
                }
            }
        }
    }
}

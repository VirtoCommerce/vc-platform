using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;

namespace VirtoCommerce.Foundation.Frameworks
{
    public static class UnityExtensions
    {
        public static void RegisterService<T>(this IUnityContainer container, string serviceUri, string endpointName)
        {
            // changing endpoint name on the fly. Every endpoint should be paired with https version.
            if (serviceUri.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                endpointName = "https" + endpointName;
            }

            var endpoint = GetEndpointByName(endpointName);
            if (endpoint == null)
            {
                throw new ApplicationException(String.Format("Endpoint configuration is missing \"{0}\"", endpointName));
            }

            if (endpoint.Address == null)
            {
                throw new ApplicationException(String.Format("Endpoint configuration \"{0}\" is missing address", endpointName));
            }

            var factory = new ChannelFactory<T>(endpointName);
            if (factory.Endpoint.Address == null && !string.IsNullOrEmpty(serviceUri))
            {
                factory.Endpoint.Address = new EndpointAddress(serviceUri);
            }

            // Register service factory Instance
            container.RegisterInstance(factory);

            // Register service factory function
            container.RegisterType<T>(
                    new InjectionFactory(c => c.Resolve<ChannelFactory<T>>().CreateChannel()));
        }

        private static ChannelEndpointElement GetEndpointByName(string name)
        {
            var clientSection = (ClientSection)ConfigurationManager.GetSection("system.serviceModel/client");
            for (var index = 0; index < clientSection.Endpoints.Count; index++)
            {
                var endpoint = clientSection.Endpoints[index];
                if (endpoint.Name == name)
                    return endpoint;
            }

            return null;
        }

        private static Uri GetConnectionString(string baseUriName, string relativeUri)
        {
            if (ConfigurationManager.ConnectionStrings[baseUriName] != null && !(relativeUri.StartsWith("http://") || relativeUri.StartsWith("https://")))
            {
                var connection = ConfigurationManager.ConnectionStrings[baseUriName].ConnectionString;
                return new Uri(new Uri(connection), relativeUri);
            }
            else if (!string.IsNullOrEmpty(baseUriName) && (baseUriName.StartsWith("http://") || baseUriName.StartsWith("https://")))
            {
                return new Uri(new Uri(baseUriName), relativeUri);
            }

            return new Uri(relativeUri);
        }
    }
}

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace VirtoCommerce.Platform.Web.Extensions
{
    public static class ConfigurationExtensions
    {
        private const int PortNotFound = -1;

        //This code is mostly taken from https://github.com/dotnet/aspnetcore/blob/master/src/Middleware/HttpsPolicy/src/HttpsRedirectionMiddleware.cs
        public static int TryGetHttpsPort(this IConfiguration configuration)
        {
            // Order for finding the HTTPS port:
            // 1. Set in the HttpsRedirectionOptions
            // 2. HTTPS_PORT environment variable
            // 3. IServerAddressesFeature
            // 4. Fail if not sets

            var nullablePort = configuration.GetValue<int?>("HTTPS_PORT") ?? configuration.GetValue<int?>("ANCM_HTTPS_PORT");
            if (nullablePort.HasValue)
            {
                var port = nullablePort.Value;
                return port;
            }
            var addresses = configuration[WebHostDefaults.ServerUrlsKey];

            if (addresses == null)
            {
                return PortNotFound;
            }
            foreach (var address in addresses.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var bindingAddress = BindingAddress.Parse(address);
                if (bindingAddress.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase))
                {
                    // If we find multiple different https ports specified, throw
                    if (nullablePort.HasValue && nullablePort != bindingAddress.Port)
                    {
                        return PortNotFound;
                    }
                    else
                    {
                        nullablePort = bindingAddress.Port;
                    }
                }
            }
            if (nullablePort.HasValue)
            {
                var port = nullablePort.Value;
                return port;
            }
            return PortNotFound;
        }

        public static bool IsHttpsServerUrlSet(this IConfiguration configuration)
        {
            return configuration.TryGetHttpsPort() != PortNotFound;
        }
    }

}


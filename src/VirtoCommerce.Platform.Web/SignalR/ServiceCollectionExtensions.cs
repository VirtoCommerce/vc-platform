
using System;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.SignalR;
using VirtoCommerce.Platform.Web.Redis;

namespace VirtoCommerce.Platform.Web.SignalR
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSignalR(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection(SignalRConfiguration.SectionName);

            var serviceBuilder = services.AddSignalR().AddNewtonsoftJsonProtocol();

            // SignalR scalability configuration. RedisBackplane (default provider) will be activated only when RedisConnectionString is set
            // otherwise no any SignalR scaling options will be used
            if (section[SignalRConfiguration.ScalabilityProvider] == SignalRConfiguration.AzureSignalRService)
            {
                serviceBuilder.AddAzureSignalR(services, configuration);
            }
            else
            {
                serviceBuilder.AddRedisBackplane(services, configuration);
            }
        }

        public static void AddAzureSignalR(this ISignalRServerBuilder builder, IServiceCollection services, IConfiguration configuration)
        {
            var options = new AzureSignalROptions();

            var sectionName = $"{SignalRConfiguration.SectionName}:{SignalRConfiguration.AzureSignalRService}";
            var section = configuration.GetSection(sectionName);
            section.Bind(options);

            services.AddOptions<AzureSignalROptions>().Bind(section).ValidateDataAnnotations();

            var azureSignalRConnectionString = options.ConnectionString;
            if (string.IsNullOrEmpty(azureSignalRConnectionString))
            {
                throw new InvalidOperationException($"{sectionName}  must be set");
            }
            builder.AddAzureSignalR(o =>
            {
                o.Endpoints = new [] { new ServiceEndpoint(azureSignalRConnectionString) };
            });
        }
    }
}

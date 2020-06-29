
using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.SignalR;

namespace VirtoCommerce.Platform.Web.SignalR
{
    public static class ServiceCollectionExtensions
    {
        public static readonly SignalROptions _signalROptions = new SignalROptions();

        public static void AddSignalR(this IServiceCollection services, IConfiguration configuration)
        {
            var signalRSection = configuration.GetSection("SignalR");
            signalRSection.Bind(_signalROptions);

            var signalRServiceBuilder = services.AddSignalR().AddNewtonsoftJsonProtocol();

            // SignalR scalability configuration. RedisBackplane (default provider) will be activated only when RedisConnectionString is set
            // otherwise no any SignalR scaling options will be used
            if (_signalROptions.ScalabilityProvider == SignalRScalabilityProvider.AzureSignalRService)
            {
                signalRServiceBuilder.AddAzureSignalR(configuration);
            }
            else
            {
                signalRServiceBuilder.AddRedisBackplane(configuration);
            }
        }

        public static void AddAzureSignalR(this ISignalRServerBuilder builder, IConfiguration configuration)
        {
            var azureSignalRConnectionString = _signalROptions.AzureSignalRService.ConnectionString;
            if (string.IsNullOrEmpty(azureSignalRConnectionString))
            {
                throw new InvalidOperationException("SignalR:AzureSignalRService:ConnectionString  must be set");
            }
            builder.AddAzureSignalR(options =>
            {
                options.Endpoints = new [] { new ServiceEndpoint(azureSignalRConnectionString) };
            });
        }

        public static void AddRedisBackplane(this ISignalRServerBuilder builder, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("RedisConnectionString");
            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                builder.AddStackExchangeRedis(redisConnectionString, options => options.Configuration.ChannelPrefix = _signalROptions.RedisBackplane.ChannelName ?? "VirtoCommerceChannel");
            }
        }
    }
}

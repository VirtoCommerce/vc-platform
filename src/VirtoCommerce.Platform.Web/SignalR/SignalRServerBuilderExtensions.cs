
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.SignalR
{
    public static class SignalRServerBuilderExtensions
    {
        public static void AddAzureSignalR(this ISignalRServerBuilder builder, IConfiguration configuration)
        {
            var services = builder.Services;
            services.AddOptions<AzureSignalRServiceOptions>().Bind(configuration.GetSection("SignalR:AzureSignalRService")).ValidateDataAnnotations();
            var azureSignalRServiceOptions = new AzureSignalRServiceOptions();
            configuration.GetSection("SignalR:AzureSignalRService").Bind(azureSignalRServiceOptions);

            builder.AddAzureSignalR(options =>
            {
                options.Endpoints = new ServiceEndpoint[] { new ServiceEndpoint(azureSignalRServiceOptions.ConnectionString) };
            });
        }

        public static void AddRedisBackplane(this ISignalRServerBuilder builder, IConfiguration configuration)
        {
            var services = builder.Services;
            var redisConnectionString = configuration.GetConnectionString("RedisConnectionString");
            services.AddOptions<SignalRRedisBackplaneOptions>().Bind(configuration.GetSection("SignalR:RedisBackplane")).ValidateDataAnnotations();
            var signalRRedisBackplaneOptions = new SignalRRedisBackplaneOptions();
            configuration.GetSection("SignalR:RedisBackplane").Bind(signalRRedisBackplaneOptions);

            if (!redisConnectionString.IsNullOrEmpty())
            {
                builder.AddStackExchangeRedis(redisConnectionString, options => options.Configuration.ChannelPrefix = signalRRedisBackplaneOptions.ChannelName );
            }
        }
    }
}

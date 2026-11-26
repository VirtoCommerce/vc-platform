using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Data.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    public static class SignalRBuilderExtensions
    {
        /// <summary>
        /// Registers push notifications. The configuration decides the mode, with no switch of its own:
        /// <list type="bullet">
        /// <item>with <c>ConnectionStrings:RedisConnectionString</c>, the notification history is kept in Redis, shared by all instances;
        /// without it, each instance keeps its own in memory;</item>
        /// <item>with <c>PushNotifications:AzureSignalRService:ConnectionString</c>, the hub runs on Azure SignalR Service;
        /// otherwise, with Redis, instances deliver each other's messages through the Redis backplane.</item>
        /// </list>
        /// </summary>
        public static ISignalRServerBuilder AddPushNotifications(this ISignalRServerBuilder builder, IConfiguration configuration)
        {
            var section = configuration.GetSection("PushNotifications");
            var options = section.Get<PushNotificationOptions>() ?? new PushNotificationOptions();
            var redisConnectionString = configuration.GetConnectionString("RedisConnectionString");
            var hasRedis = !string.IsNullOrEmpty(redisConnectionString);

            builder.Services.AddOptions<PushNotificationOptions>().Bind(section);
            builder.Services.AddSingleton<IUserIdProvider, PushNotificationUserIdProvider>();
            builder.Services.AddSingleton<IPushNotificationManager, PushNotificationManager>();

            if (hasRedis)
            {
                builder.Services.AddSingleton<IPushNotificationStorage, RedisPushNotificationStorage>();
            }
            else
            {
                builder.Services.AddSingleton<IPushNotificationStorage, PushNotificationInMemoryStorage>();
            }

            var azureSignalRConnectionString = options.AzureSignalRService?.ConnectionString;
            if (IsConfigured(azureSignalRConnectionString))
            {
                builder.AddAzureSignalR(o => o.Endpoints = [new ServiceEndpoint(azureSignalRConnectionString)]);
            }
            else if (hasRedis)
            {
                var channelName = PushNotificationRedisOptions.GetChannelName(options.RedisBackplane);
                builder.AddStackExchangeRedis(redisConnectionString, o => o.Configuration.ChannelPrefix = RedisChannel.Literal(channelName));
            }

            return builder;
        }

        // Earlier appsettings.json shipped a placeholder ("Endpoint=https://{app name}.service.signalr.net;...") that was ignored
        // unless ScalabilityMode selected Azure. A copy of it must not switch the hub to Azure SignalR Service now.
        private static bool IsConfigured(string connectionString)
        {
            return !string.IsNullOrWhiteSpace(connectionString) && !connectionString.Contains('{');
        }
    }
}

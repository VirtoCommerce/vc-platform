using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.Redis;
using VirtoCommerce.Platform.Core.SignalR;
using VirtoCommerce.Platform.Web.PushNotifications.Scalability;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPushNotifications(this IServiceCollection services, IConfiguration configuration)
        {
            var signalRSection = configuration.GetSection(SignalRConfiguration.SectionName);

            services.AddSingleton<IPushNotificationStorage, PushNotificationInMemoryStorage>();

            var redisConnectionString = configuration.GetConnectionString(RedisConfiguration.ConnectionStringName);
            if (signalRSection[SignalRConfiguration.ScalabilityProvider] == SignalRConfiguration.AzureSignalRService || !string.IsNullOrEmpty(redisConnectionString))
            {
                services.AddSingleton<IPushNotificationManager, ScalablePushNotificationManager>();

                services.AddSingleton<IHubConnectionBuilder>(new HubConnectionBuilder());
                services.AddHostedService<PushNotificationHandler>();
            }
            else
            {
                services.AddSingleton<IPushNotificationManager, PushNotificationManager>();
            }

            return services;
        }
    }
}

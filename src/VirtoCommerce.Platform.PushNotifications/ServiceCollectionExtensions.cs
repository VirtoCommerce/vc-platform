using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.JsonConverters;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Core.SignalR;
using VirtoCommerce.Platform.PushNotifications.Scalability;

namespace VirtoCommerce.Platform.PushNotifications
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPushNotifications(this IServiceCollection services, IConfiguration configuration)
        {
            var signalRSection = configuration.GetSection("SignalR");
            var signalROptions = new SignalROptions();
            signalRSection.Bind(signalROptions);

            var pushNotificationsSection = configuration.GetSection("PushNotifications");
            var pushNotificationsOptions = new PushNotificationOptions();
            pushNotificationsSection.Bind(pushNotificationsOptions);

            services.AddSingleton<IPushNotificationStorage, PushNotificationInMemoryStorage>();

            var redisConnectionString = configuration.GetConnectionString("RedisConnectionString");
            if (signalROptions.ScalabilityProvider == SignalRScalabilityProvider.AzureSignalRService || !string.IsNullOrEmpty(redisConnectionString))
            {
                services.AddSingleton<IHubConnectionBuilder>(new HubConnectionBuilder()
                    .AddNewtonsoftJsonProtocol(o => o.PayloadSerializerSettings.Converters.Add(new PolymorphJsonConverter()))
                    .WithUrl(pushNotificationsOptions.Scalability.HubUrl)
                    .WithAutomaticReconnect());
                services.AddSingleton<IPushNotificationManager, ScalablePushNotificationManager>();
            }
            else
            {
                services.AddSingleton<IPushNotificationManager, PushNotificationManager>();
            }

            return services;
        }
    }
}

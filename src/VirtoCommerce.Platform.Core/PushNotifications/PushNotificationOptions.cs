using System;

namespace VirtoCommerce.Platform.Core.PushNotifications
{
    /// <summary>
    /// The <c>PushNotifications</c> configuration section. The scale-out mode follows the configured services:
    /// Azure SignalR Service when <see cref="AzureSignalRService"/> has a connection string, otherwise the Redis backplane
    /// when <c>ConnectionStrings:RedisConnectionString</c> is set. With Redis, the notification history is kept in Redis.
    /// </summary>
    public class PushNotificationOptions
    {
        [Obsolete("Not used. The scale-out mode follows the configuration: Azure SignalR Service when PushNotifications:AzureSignalRService:ConnectionString is set, otherwise the Redis backplane when ConnectionStrings:RedisConnectionString is set.", DiagnosticId = "VC0016", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public string ScalabilityMode { get; set; } = "None";

        [Obsolete("Not used. Platform instances share the notification history through Redis (ConnectionStrings:RedisConnectionString) instead of a hub client with an api key.", DiagnosticId = "VC0016", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public string HubUrl { get; set; }

        /// <summary>
        /// Tells the admin UI to connect to the hub with WebSockets only, skipping negotiation. Use it behind a load balancer without
        /// sticky sessions, where negotiation and the connection could reach different instances.
        /// </summary>
        public bool ForceWebSockets { get; set; }

        /// <summary>
        /// The maximum number of notifications kept for one user; older ones are removed first. 0 or less keeps all.
        /// </summary>
        public int MaxNotificationsPerUser { get; set; } = 20;

        /// <summary>
        /// How long a notification is kept after it was created. <see cref="TimeSpan.Zero"/> or less turns the retention off.
        /// </summary>
        public TimeSpan RetentionPeriod { get; set; } = TimeSpan.FromHours(24);

        public PushNotificationRedisOptions RedisBackplane { get; set; } = new();

        public PushNotificationAzureSignalROptions AzureSignalRService { get; set; } = new();
    }

    public class PushNotificationRedisOptions
    {
        /// <summary>
        /// The prefix of the backplane channels and of the notification history keys. Environments that share one Redis must use different names.
        /// </summary>
        public string ChannelName { get; set; } = "VirtoCommerceChannel";
    }

    public class PushNotificationAzureSignalROptions
    {
        /// <summary>
        /// The Azure SignalR Service connection string. When set, the hub runs on Azure SignalR Service.
        /// </summary>
        public string ConnectionString { get; set; }
    }
}

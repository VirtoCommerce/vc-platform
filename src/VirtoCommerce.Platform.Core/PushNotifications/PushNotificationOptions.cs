using System;

namespace VirtoCommerce.Platform.Core.PushNotifications
{
    public class PushNotificationOptions
    {
        public string ScalabilityMode { get; set; } = "None";

        /// <summary>
        /// Legacy synchronization of the notification storage between instances: the URL of the platform /pushNotificationHub with an api_key
        /// whose user receives all notifications. Used only in a scale-out mode without <c>ConnectionStrings:RedisConnectionString</c>;
        /// with Redis, all instances keep the notifications in Redis and this value is ignored.
        /// </summary>
        public string HubUrl { get; set; }

        public bool ForceWebSockets { get; set; } = false;

        /// <summary>
        /// The maximum number of notifications kept for one user (creator); older ones are removed first. 0 or less keeps all.
        /// </summary>
        public int MaxNotificationsPerUser { get; set; } = 20;

        /// <summary>
        /// How long a notification is kept after it was created. <see cref="TimeSpan.Zero"/> or less keeps notifications until restart.
        /// </summary>
        public TimeSpan RetentionPeriod { get; set; } = TimeSpan.FromHours(24);
    }
}

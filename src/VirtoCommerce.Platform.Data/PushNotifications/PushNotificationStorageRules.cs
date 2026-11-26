using System;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Data.PushNotifications
{
    /// <summary>
    /// The rules every <see cref="IPushNotificationStorage"/> of the platform applies, so notifications behave the same in memory and in Redis:
    /// <list type="bullet">
    /// <item>a notification with a known id replaces the stored one; any other is added, even with the same content as an earlier one;</item>
    /// <item>a user keeps the newest <see cref="PushNotificationOptions.MaxNotificationsPerUser"/> notifications;</item>
    /// <item>notifications created more than <see cref="PushNotificationOptions.RetentionPeriod"/> ago are removed and not returned.</item>
    /// </list>
    /// </summary>
    public static class PushNotificationStorageRules
    {
        /// <summary>
        /// The creation time before which notifications are expired, or null when the retention is off.
        /// </summary>
        public static DateTime? GetExpiredBefore(PushNotificationOptions options, TimeProvider timeProvider)
        {
            return options.RetentionPeriod > TimeSpan.Zero
                ? timeProvider.GetUtcNow().UtcDateTime - options.RetentionPeriod
                : null;
        }

        /// <summary>
        /// The number of notifications kept per user; 0 keeps all.
        /// </summary>
        public static int GetMaxPerUser(PushNotificationOptions options)
        {
            return Math.Max(options.MaxNotificationsPerUser, 0);
        }
    }
}

using System;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Data.PushNotifications
{
    /// <summary>
    /// The rules every <see cref="IPushNotificationStorage"/> of the platform applies, so notifications behave the same in memory and in Redis:
    /// <list type="bullet">
    /// <item>a notification with a known id replaces the stored one; any other is added, even with the same content as an earlier one;</item>
    /// <item>a user keeps the <see cref="PushNotificationOptions.MaxNotificationsPerUser"/> most recently saved notifications;</item>
    /// <item>notifications last saved more than <see cref="PushNotificationOptions.RetentionPeriod"/> ago are removed and not returned.</item>
    /// </list>
    /// Both count from the last save, not from <see cref="PushNotification.Created"/>: a long job reuses one notification and keeps its
    /// <see cref="PushNotification.Created"/> (shown as its start), and it must stay in the history while it reports and after it ends.
    /// </summary>
    public static class PushNotificationStorageRules
    {
        /// <summary>
        /// The save time before which notifications are expired, or null when the retention is off.
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

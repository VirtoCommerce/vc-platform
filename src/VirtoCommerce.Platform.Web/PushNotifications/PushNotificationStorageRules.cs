using System;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    /// <summary>
    /// The rules every <see cref="IPushNotificationStorage"/> of the platform applies, so notifications behave the same in memory and in Redis:
    /// <list type="bullet">
    /// <item>a notification with a known id replaces the stored one;</item>
    /// <item>a new notification with the same content as the same user's last one increases its <see cref="PushNotification.RepeatCount"/>;</item>
    /// <item>a user keeps the newest <see cref="PushNotificationOptions.MaxNotificationsPerUser"/> notifications;</item>
    /// <item>notifications created more than <see cref="PushNotificationOptions.RetentionPeriod"/> ago are removed and not returned.</item>
    /// </list>
    /// </summary>
    public static class PushNotificationStorageRules
    {
        /// <summary>
        /// The creation time before which notifications are expired, or null when they are kept until removed by the limit.
        /// </summary>
        public static DateTime? GetExpiredBefore(PushNotificationOptions options)
        {
            return options.RetentionPeriod > TimeSpan.Zero
                ? DateTime.UtcNow - options.RetentionPeriod
                : null;
        }

        /// <summary>
        /// The number of notifications kept per user; 0 keeps all.
        /// </summary>
        public static int GetMaxPerUser(PushNotificationOptions options)
        {
            return Math.Max(options.MaxNotificationsPerUser, 0);
        }

        /// <summary>
        /// When <paramref name="notification"/> repeats the user's <paramref name="last"/> notification, counts the repeat on <paramref name="last"/>
        /// and returns true; the caller then saves <paramref name="last"/> instead of <paramref name="notification"/>.
        /// </summary>
        public static bool TryCountRepeat(PushNotification last, PushNotification notification)
        {
            if (last == null || last.Id == notification.Id || last.Creator != notification.Creator || !last.ItHasSameContent(notification))
            {
                return false;
            }

            last.IsNew = true;
            last.RepeatCount++;
            last.Created = DateTime.UtcNow;

            return true;
        }
    }
}

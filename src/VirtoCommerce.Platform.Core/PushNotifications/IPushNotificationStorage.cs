using System;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.PushNotifications
{
    /// <summary>
    /// Keeps the push notification history that users read in the back office.
    /// </summary>
    public interface IPushNotificationStorage
    {
        [Obsolete("Use SavePushNotificationAsync() instead.", DiagnosticId = "VC0016", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        void SavePushNotification(PushNotification notification)
        {
            SavePushNotificationAsync(notification).GetAwaiter().GetResult();
        }

        Task SavePushNotificationAsync(PushNotification notification);

        [Obsolete("Use SearchPushNotificationsAsync() instead.", DiagnosticId = "VC0016", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        PushNotificationSearchResult SearchPushNotifications(string userId, PushNotificationSearchCriteria criteria)
        {
            return SearchPushNotificationsAsync(userId, criteria).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns the notifications created by <paramref name="userId"/> (the user name) that match <paramref name="criteria"/>.
        /// </summary>
        Task<PushNotificationSearchResult> SearchPushNotificationsAsync(string userId, PushNotificationSearchCriteria criteria, CancellationToken cancellationToken = default);
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
        [SuppressMessage("Reliability", "S4462", Justification = "A synchronous method kept for existing callers; it blocks on the asynchronous one by design.")]
        void SavePushNotification(PushNotification notification)
        {
            SavePushNotificationAsync(notification).GetAwaiter().GetResult();
        }

        Task SavePushNotificationAsync(PushNotification notification);

        [Obsolete("Use SearchPushNotificationsAsync() instead.", DiagnosticId = "VC0016", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        [SuppressMessage("Reliability", "S4462", Justification = "A synchronous method kept for existing callers; it blocks on the asynchronous one by design.")]
        PushNotificationSearchResult SearchPushNotifications(string userId, PushNotificationSearchCriteria criteria)
        {
            return SearchPushNotificationsAsync(userId, criteria).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Returns the notifications created by <paramref name="userId"/> (the user name) that match <paramref name="criteria"/>.
        /// The default implementation calls <see cref="SearchPushNotifications"/>, so an implementation built against an earlier version keeps
        /// loading and working. An implementation must override at least one of the two.
        /// </summary>
        Task<PushNotificationSearchResult> SearchPushNotificationsAsync(string userId, PushNotificationSearchCriteria criteria, CancellationToken cancellationToken = default)
        {
#pragma warning disable VC0016 // Fallback for implementations that only have the synchronous method
            return Task.FromResult(SearchPushNotifications(userId, criteria));
#pragma warning restore VC0016
        }

        /// <summary>
        /// Marks the new notifications of <paramref name="userId"/> (the user name) as read and returns them.
        /// The default implementation saves each of them again.
        /// </summary>
        async Task<IList<PushNotification>> MarkAllAsReadAsync(string userId, CancellationToken cancellationToken = default)
        {
            var result = await SearchPushNotificationsAsync(userId, new PushNotificationSearchCriteria { OnlyNew = true, Take = int.MaxValue }, cancellationToken);

            foreach (var notification in result.NotifyEvents)
            {
                notification.IsNew = false;
                await SavePushNotificationAsync(notification);
            }

            return result.NotifyEvents;
        }
    }
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.PushNotifications
{
    public interface IPushNotificationManager
    {
        void Send(PushNotification notification);
        Task SendAsync(PushNotification notification);
        PushNotificationSearchResult SearchNotifies(string userId, PushNotificationSearchCriteria criteria);

        /// <summary>
        /// Returns the notifications created by <paramref name="userId"/> (the user name) that match <paramref name="criteria"/>.
        /// The default implementation calls <see cref="SearchNotifies"/>, so an existing implementation keeps working.
        /// </summary>
        Task<PushNotificationSearchResult> SearchNotifiesAsync(string userId, PushNotificationSearchCriteria criteria, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(SearchNotifies(userId, criteria));
        }

        /// <summary>
        /// Marks the new notifications of <paramref name="userId"/> (the user name) as read and returns them.
        /// The default implementation sends each of them again, so an existing implementation keeps working.
        /// </summary>
        async Task<IList<PushNotification>> MarkAllAsReadAsync(string userId, CancellationToken cancellationToken = default)
        {
            var result = await SearchNotifiesAsync(userId, new PushNotificationSearchCriteria { OnlyNew = true, Take = int.MaxValue }, cancellationToken);

            foreach (var notification in result.NotifyEvents)
            {
                notification.IsNew = false;
                await SendAsync(notification);
            }

            return result.NotifyEvents;
        }
    }
}

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
    }
}

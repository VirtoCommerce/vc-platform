using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    public class PushNotificationManager : IPushNotificationManager
    {
        private readonly IPushNotificationStorage _storage;
        private readonly IHubContext<PushNotificationHub> _hubContext;

        public PushNotificationManager(IPushNotificationStorage storage, IHubContext<PushNotificationHub> hubContext)
        {
            _hubContext = hubContext;
            _storage = storage;
        }

        public virtual PushNotificationSearchResult SearchNotifies(string userId, PushNotificationSearchCriteria criteria)
        {
            return SearchNotifiesAsync(userId, criteria).GetAwaiter().GetResult();
        }

        public virtual Task<PushNotificationSearchResult> SearchNotifiesAsync(string userId, PushNotificationSearchCriteria criteria, CancellationToken cancellationToken = default)
        {
            return _storage.SearchPushNotificationsAsync(userId, criteria, cancellationToken);
        }

        /// <summary>
        /// Marks the user's new notifications as read in the storage, without sending them to the hub again.
        /// </summary>
        public virtual Task<IList<PushNotification>> MarkAllAsReadAsync(string userId, CancellationToken cancellationToken = default)
        {
            return _storage.MarkAllAsReadAsync(userId, cancellationToken);
        }

        public virtual void Send(PushNotification notification)
        {
            SendAsync(notification).GetAwaiter().GetResult();
        }

        public virtual async Task SendAsync(PushNotification notification)
        {
            await _storage.SavePushNotificationAsync(notification);

            if (_hubContext != null && notification.Creator != null)
            {
                await _hubContext.Clients.User(notification.Creator).SendAsync("Send", notification);
                await _hubContext.Clients.All.SendAsync("SendSystemEvents", notification);
            }
        }
    }
}

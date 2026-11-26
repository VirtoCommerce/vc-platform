using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    public class PushNotificationManager : IPushNotificationManager
    {
        private readonly IPushNotificationStorage _storage;
        private readonly IHubContext<PushNotificationHub> _hubContext;
        private readonly ILogger<PushNotificationManager> _logger;

        public PushNotificationManager(IPushNotificationStorage storage, IHubContext<PushNotificationHub> hubContext)
            : this(storage, hubContext, null)
        {
        }

        public PushNotificationManager(IPushNotificationStorage storage, IHubContext<PushNotificationHub> hubContext, ILogger<PushNotificationManager> logger)
        {
            _hubContext = hubContext;
            _storage = storage;
            _logger = logger ?? NullLogger<PushNotificationManager>.Instance;
        }

        [SuppressMessage("Reliability", "S4462", Justification = "A synchronous method kept for existing callers; it blocks on the asynchronous one by design.")]
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
            try
            {
                await _storage.SavePushNotificationAsync(notification);
            }
            catch (Exception ex)
            {
                // The history is secondary to the work that reports through it: a failed write (for example, a Redis timeout)
                // must not fail a background job, and the user still gets the live notification below.
                _logger.LogError(ex, "Could not save push notification {Id} of {Creator} to the history", notification.Id, notification.Creator);
            }

            if (_hubContext != null && notification.Creator != null)
            {
                await _hubContext.Clients.User(notification.Creator).SendAsync("Send", notification);
                await _hubContext.Clients.All.SendAsync("SendSystemEvents", notification);
            }
        }
    }
}

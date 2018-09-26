using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    [CLSCompliant(false)]
    public class PushNotificationManager : IPushNotificationManager
    {
        private readonly IPushNotificationService _pushNotificationService;
        private readonly IHubContext _hubSignalR;

        public PushNotificationManager(IPushNotificationService pushNotificationService, IHubContext hubSignalR)
        {
            _pushNotificationService = pushNotificationService;
            _hubSignalR = hubSignalR;
        }

        public void Upsert(PushNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            _pushNotificationService.SaveChanges(new[] { notification });
            _hubSignalR.Clients.All.notification(notification);
        }

        public PushNotificationSearchResult SearchNotifies(string userId, PushNotificationSearchCriteria criteria)
        {
            return _pushNotificationService.SearchPushNotification(userId, criteria);
        }
    }
}

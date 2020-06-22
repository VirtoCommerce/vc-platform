using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    public class PushNotificationManager : IPushNotificationManager
    {
        private readonly IHubContext<PushNotificationHub> _hubContext;
        private readonly IPushNotificationStorage _storage;
        public PushNotificationManager(IHubContext<PushNotificationHub> hubContext, IPushNotificationStorage storage)
        {
            _hubContext = hubContext;            
            _storage = storage;
        }

        public PushNotificationSearchResult SearchNotifies(string userId, PushNotificationSearchCriteria criteria)
        {            
            var retVal = _storage.SearchNotifies(userId, criteria);
            return retVal;
        }

        public void Send(PushNotification notification)
        {
            SendAsync(notification).GetAwaiter().GetResult();
        }

        public async Task SendAsync(PushNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }
           
            _storage.AddOrUpdate(notification);

            if (_hubContext != null)
            {
                await _hubContext.Clients.All.SendAsync("Send", notification );                
            }
        }

    }

}

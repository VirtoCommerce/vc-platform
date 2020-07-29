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
            var retVal = _storage.SearchPushNotifications(userId, criteria);
            return retVal;
        }

        public virtual void Send(PushNotification notification)
        {
            SendAsync(notification).GetAwaiter().GetResult();
        }

        public virtual async Task SendAsync(PushNotification notification)
        {
            await _storage.SavePushNotificationAsync(notification);

            if (_hubContext != null)
            {
                await _hubContext.Clients.All.SendAsync("Send", notification);
            }
        }
    }

}

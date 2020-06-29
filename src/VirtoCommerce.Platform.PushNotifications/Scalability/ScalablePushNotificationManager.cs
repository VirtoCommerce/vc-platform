using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.PushNotifications.Scalability
{
    public class ScalablePushNotificationManager: PushNotificationManager
    {
        private readonly IPushNotificationStorage _storage;
        private readonly string _serverId = Guid.NewGuid().ToString("N");

        public ScalablePushNotificationManager(IPushNotificationStorage storage, IHubContext<PushNotificationHub> hubContext, IHubConnectionBuilder hubConnectionBuilder): base(storage, hubContext)
        {
            _storage = storage;

            var hubConnection = hubConnectionBuilder.Build();
            hubConnection.StartAsync();
            hubConnection.On<PushNotification>("Send", pushNotification =>
            {
                if (pushNotification.ServerId != _serverId)
                {
                    storage.SavePushNotification(pushNotification);
                }
            });
        }

        public override async Task SendAsync(PushNotification notification)
        {
            notification.ServerId = _serverId;
            await base.SendAsync(notification);
        }
    }
}

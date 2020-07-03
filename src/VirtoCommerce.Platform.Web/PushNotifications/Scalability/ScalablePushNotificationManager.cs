using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications.Scalability
{
    public class ScalablePushNotificationManager: PushNotificationManager
    {
        public static string ServerId { get; } = Guid.NewGuid().ToString("N");

        public ScalablePushNotificationManager(IPushNotificationStorage storage, IHubContext<PushNotificationHub> hubContext) : base(storage, hubContext)
        {
        }


        public override Task SendAsync(PushNotification notification)
        {
            notification.ServerId = ServerId;
            return base.SendAsync(notification);
        }
    }
}

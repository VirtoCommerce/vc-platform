using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications.Scalability
{
    [Obsolete("Not used. The platform registers PushNotificationManager in every mode; instances share the notification history through Redis.", DiagnosticId = "VC0016", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
    public class ScalablePushNotificationManager: PushNotificationManager
    {
        public static string ServerId { get; } = $"{Environment.MachineName}_{Guid.NewGuid():N}";

        public ScalablePushNotificationManager(IPushNotificationStorage storage
            , IHubContext<PushNotificationHub> hubContext)
            : base(storage, hubContext)
        {
        }


        public override async Task SendAsync(PushNotification notification)
        {
            notification.ServerId = ServerId;

            await base.SendAsync(notification);
        }
    }
}

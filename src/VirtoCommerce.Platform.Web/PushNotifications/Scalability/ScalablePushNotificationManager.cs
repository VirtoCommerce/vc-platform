using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications.Scalability
{
    public class ScalablePushNotificationManager: PushNotificationManager
    {
        private readonly ILogger<ScalablePushNotificationManager> _log;
        public static string ServerId { get; } = Guid.NewGuid().ToString("N");

        public ScalablePushNotificationManager(IPushNotificationStorage storage
            , IHubContext<PushNotificationHub> hubContext
            , ILogger<ScalablePushNotificationManager> log)
            : base(storage, hubContext)
        {
            _log = log;
        }


        public override async Task SendAsync(PushNotification notification)
        {
            notification.ServerId = ServerId;
            await base.SendAsync(notification);
            _log.LogInformation($"{nameof(PushNotificationManager)}: sending push notification with {notification.Id} ID of type {notification.NotifyType} to {ServerId} server");
        }
    }
}

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications.Scalability
{
    public class ScalablePushNotificationManager: PushNotificationManager
    {
        public static string ServerId { get; } = Guid.NewGuid().ToString("N");

        private readonly IPushNotificationStorage _storage;
        private readonly IHubContext<PushNotificationHub> _hubContext;
        private readonly ILogger<ScalablePushNotificationManager> _log;
        private readonly JsonSerializer _jsonSerializer;

        public ScalablePushNotificationManager(IPushNotificationStorage storage
            , IHubContext<PushNotificationHub> hubContext
            , IOptions<MvcNewtonsoftJsonOptions> jsonOptions
            , ILogger<ScalablePushNotificationManager> log)
            : base(storage, hubContext)
        {
            _storage = storage;
            _hubContext = hubContext;
            _log = log;

            _jsonSerializer = JsonSerializer.CreateDefault(jsonOptions.Value.SerializerSettings);
        }


        public override async Task SendAsync(PushNotification notification)
        {
            await _storage.SavePushNotificationAsync(notification);

            var distributedNotification = notification.JsonConvert<DistributedPushNotification>(_jsonSerializer);
            distributedNotification.ServerId = ServerId;
            
            if (_hubContext != null)
            {
                await _hubContext.Clients.All.SendAsync("Send", distributedNotification);
            }

            _log.LogInformation($"{nameof(PushNotificationManager)}: sending push notification with {distributedNotification.Id} ID of type {distributedNotification.NotifyType} to {distributedNotification.ServerId} server");
        }
    }
}

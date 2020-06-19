using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    public class PushNotificationSignalRSynchronizer
    {
        private readonly HubConnection connection;
        private readonly IPushNotificationStorage _storage;
        public PushNotificationSignalRSynchronizer(IPushNotificationStorage storage)
        {
            _storage = storage;

            connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:10645/pushNotificationHub")
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            connection.On<PushNotification>("Send", notification =>
            {
                _storage.AddOrUpdate(notification);
            });
        }

        public async Task StartAsync()
        {
            await connection.StartAsync();
        }

    }
}

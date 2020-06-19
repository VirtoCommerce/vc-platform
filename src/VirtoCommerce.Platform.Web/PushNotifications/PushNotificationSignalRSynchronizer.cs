using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    public class PushNotificationSignalRSynchronizer : IHostedService
    {
        private HubConnection connection;
        private readonly IPushNotificationStorage _storage;
        public PushNotificationSignalRSynchronizer(IPushNotificationStorage storage)
        {
            _storage = storage;
            connection = new HubConnectionBuilder()
               .WithUrl("http://localhost:10645/pushNotificationHub")
               .WithAutomaticReconnect()               
               .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            connection.On<PushNotification>("Send", notification =>
            {
                //var str = doc.ToString();
                //var notification = JsonSerializer.Deserialize<PushNotification>(str);
                Debug.WriteLine(notification.ToString());
                //_storage.AddOrUpdate(notification);
            });

            connection.On<string>("Send2", notification =>
            {
                Debug.WriteLine(notification);
            });
        }

        public async Task StartAsync()
        {
            await connection.StartAsync();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run( async () => await ConnectWithRetryAsync(connection, cancellationToken) );
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return connection.DisposeAsync();
        }


        private static async Task<bool> ConnectWithRetryAsync(HubConnection connection, CancellationToken token)
        {
            // Keep trying to until we can start or the token is canceled.
            while (true)
            {
                try
                {
                    await connection.StartAsync(token);
                    Debug.Assert(connection.State == HubConnectionState.Connected);
                    return true;
                }
                catch when (token.IsCancellationRequested)
                {
                    return false;
                }
                catch
                {
                    // Failed to connect, trying again in 5000 ms.
                    Debug.Assert(connection.State == HubConnectionState.Disconnected);
                    await Task.Delay(5000);
                }
            }
        }
    }
}

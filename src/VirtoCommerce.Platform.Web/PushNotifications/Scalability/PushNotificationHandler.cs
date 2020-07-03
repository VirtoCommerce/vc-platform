using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications.Scalability
{
    public class PushNotificationHandler: BackgroundService, IAsyncDisposable
    {
        private readonly IPushNotificationStorage _storage;
        private readonly HubConnection _hubConnection;

        public PushNotificationHandler(IPushNotificationStorage storage, IHubConnectionBuilder hubConnectionBuilder)
        {
            _storage = storage;
            _hubConnection = hubConnectionBuilder.Build();
            // We want to continue receive notifications after reconnection until the application shutdown,
            // so we will never call Dispose() on this subscription
            _hubConnection.On<PushNotification>("Send", OnSend);
        }

        // Why not in StartAsync? Because we connect to same server, so we need to wait application start
        // https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-3/#a-small-change-makes-all-the-difference
        // An alternative is to use health checks, but it's not our case
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _hubConnection.StartAsync(stoppingToken);
        }

        protected virtual async Task OnSend(PushNotification pushNotification)
        {
            if (pushNotification.ServerId != ScalablePushNotificationManager.ServerId)
            {
                await _storage.SavePushNotificationAsync(pushNotification);
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _hubConnection.StopAsync(cancellationToken);
            return base.StopAsync(cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            await _hubConnection.DisposeAsync();
        }

        public override void Dispose()
        {
            DisposeAsync().GetAwaiter().GetResult();
            base.Dispose();
        }
    }
}

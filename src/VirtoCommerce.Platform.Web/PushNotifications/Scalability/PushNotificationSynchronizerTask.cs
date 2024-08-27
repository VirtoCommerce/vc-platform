using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications.Scalability
{
    /// <summary>
    /// This background task connects to the signalR hub (push notifications) to be able receive the notifications from other platform instances and actualize them
    /// into the local notifications storage   
    /// </summary>
    public class PushNotificationSynchronizerTask : BackgroundService
    {
        private readonly IPushNotificationStorage _storage;
        private readonly HubConnection _hubConnection;
        private readonly IDisposable _subscription;
        private readonly PushNotificationOptions _options;
        private readonly ILogger<PushNotificationSynchronizerTask> _logger;


        public PushNotificationSynchronizerTask(IPushNotificationStorage storage
            , IOptions<PushNotificationOptions> options, ILogger<PushNotificationSynchronizerTask> logger)
        {
            _logger = logger;
            _options = options.Value;

            _hubConnection = new HubConnectionBuilder().AddNewtonsoftJsonProtocol(jsonOptions =>
            {
                jsonOptions.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
                jsonOptions.PayloadSerializerSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
            })
             .WithAutomaticReconnect()
             .WithUrl(_options.HubUrl, options =>
             {
                 options.Credentials = null;
             })
             .Build();

            _subscription = _hubConnection.On<PushNotification>("SendSystemEvents", OnSend);
            _storage = storage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_hubConnection.State == HubConnectionState.Disconnected)
                {
                    // Wait until the platform is started to be able to connect to the SignalR hub
                    try
                    {
                        await _hubConnection.StartAsync(stoppingToken);

                        _logger.LogInformation("Started connection with {HubUrl}", _options.HubUrl);
                    }
                    catch (Exception ex)
                    {
                        // Raise the error to the log
                        _logger.LogError(ex, "Could not start the connection to the server {HubUrl}", _options.HubUrl);
                    }
                }
                await Task.Delay(1000);
            }
        }

        protected virtual async Task OnSend(PushNotification notification)
        {
            _logger.LogInformation("Receive Push Notification {Id}, {Title} from {Creator} {ServerId}", notification.Id, notification.Title, notification.Creator, notification.ServerId);

            if (notification.ServerId != ScalablePushNotificationManager.ServerId)
            {
                await _storage.SavePushNotificationAsync(notification);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _subscription?.Dispose();
            await _hubConnection?.StopAsync(cancellationToken);
            await base.StopAsync(cancellationToken);
        }


    }
}

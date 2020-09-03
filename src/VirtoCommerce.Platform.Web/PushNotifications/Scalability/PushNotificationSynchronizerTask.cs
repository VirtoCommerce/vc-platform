using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications.Scalability
{
    /// <summary>
    /// This background task connects to the signalR hub (push notifications) to be able receive the notifications from other platform instances and actualize them
    /// into the local notifications storage   
    /// </summary>
    public class PushNotificationSynchronizerTask: BackgroundService
    {
        private readonly IPushNotificationStorage _storage;
        private readonly HubConnection _hubConnection;
        private readonly IDisposable _subscription;


        public PushNotificationSynchronizerTask(IPushNotificationStorage storage
            , IOptions<PushNotificationOptions> options)
        {
            _hubConnection = new HubConnectionBuilder().AddNewtonsoftJsonProtocol(jsonOptions =>
            {
                jsonOptions.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
                jsonOptions.PayloadSerializerSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
            })
             .WithAutomaticReconnect()
             .WithUrl(options.Value.HubUrl)
             .Build();

            _subscription = _hubConnection.On<PushNotification>("Send", OnSend);
            _storage = storage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_hubConnection.State == HubConnectionState.Disconnected)
                {                  
                    //Wait until the platform being started to be able connect to the SignalR hub
                    try
                    {
                        await _hubConnection.StartAsync(stoppingToken);
                    }
                    catch
                    {
                        //Wait until platform is started
                    }
                }
                await Task.Delay(1000);
            }
        }

        protected virtual async Task OnSend(PushNotification notification)
        {
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

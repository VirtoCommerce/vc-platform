using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VirtoCommerce.Platform.Core.PushNotifications;


namespace VirtoCommerce.Platform.Web.PushNotifications
{
    /// <summary>
    /// HostedService to synchronize PushNotification inner storage in scalable environment with multyple instances
    /// </summary>
    public class PushNotificationSignalRSynchronizer : IHostedService
    {
        private readonly HubConnection connection;
        private readonly IPushNotificationStorage _storage;
        public PushNotificationSignalRSynchronizer(IPushNotificationStorage storage)
        { 
            _storage = storage;
            connection = new HubConnectionBuilder()
               .AddJsonProtocol(options =>
               {
                   options.PayloadSerializerOptions.Converters.Add(new PushNotificationJsonConverter());
               })
               //.AddNewtonsoftJsonProtocol()
               // TODO: get Public Url from configuration
               .WithUrl("http://localhost:10645/pushNotificationHub") 
               .WithAutomaticReconnect()               
               .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            //for polymorphic need to do this
            //https://docs.microsoft.com/ru-ru/dotnet/standard/serialization/system-text-json-converters-how-to#support-polymorphic-deserialization
            //https://stackoverflow.com/questions/58074304/is-polymorphic-deserialization-possible-in-system-text-json/59744873#59744873
            connection.On<PushNotification>("Send", notification =>
            { 
                // TODO: Add custom converter to deserialization any derived from PushNotification class object               
                _storage.AddOrUpdate(notification);
            });

            //connection.On<ModulePushNotification>("Send", notification =>
            //{
            //    Debug.WriteLine(notification.Id);
            //});
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

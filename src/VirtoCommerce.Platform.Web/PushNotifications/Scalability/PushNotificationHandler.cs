using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.PushNotifications;

namespace VirtoCommerce.Platform.Web.PushNotifications.Scalability
{
    public class PushNotificationHandler: BackgroundService, IAsyncDisposable
    {
        private readonly IPushNotificationStorage _storage;
        private readonly IHubConnectionBuilder _hubConnectionBuilder;
        private readonly MvcNewtonsoftJsonOptions _jsonOptions;
        private readonly PushNotificationOptions _pushNotificationsOptions;
        private readonly ILogger<PushNotificationHandler> _log;
        private readonly TelemetryClient _telemetryClient;
        private HubConnection _hubConnection;
        private IDisposable _subscription;
        private bool _stoppingOrDisposing;

        public PushNotificationHandler(IPushNotificationStorage storage
            , IHubConnectionBuilder hubConnectionBuilder
            , IOptions<PushNotificationOptions> pushNotificationsOptions
            , IOptions<MvcNewtonsoftJsonOptions> jsonOptions
            , ILogger<PushNotificationHandler> log
            , TelemetryClient telemetryClient)
        {
            _storage = storage;
            _hubConnectionBuilder = hubConnectionBuilder;
            _jsonOptions = jsonOptions.Value;
            _pushNotificationsOptions = pushNotificationsOptions.Value;
            _log = log;
            _telemetryClient = telemetryClient;
        }

        // Why not in StartAsync? Because we connect to same server, so we need to wait application start
        // https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-3/#a-small-change-makes-all-the-difference
        // An alternative is to use health checks, but it's not our case
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var pushNotificationHubUrl = _pushNotificationsOptions.Scalability.HubUrl;

            _log.LogInformation($"{nameof(PushNotificationHandler)}: attempt connecting to {pushNotificationHubUrl} hub with {ScalablePushNotificationManager.ServerId} server ID");
            _telemetryClient.TrackEvent("PushNotificationsHubConnecting", new Dictionary<string, string>
            {
                {"hubUrl", pushNotificationHubUrl},
                {"serverId", ScalablePushNotificationManager.ServerId}
            });

            _hubConnection = _hubConnectionBuilder
                .AddNewtonsoftJsonProtocol(o => o.PayloadSerializerSettings = _jsonOptions.SerializerSettings)
                .WithUrl(pushNotificationHubUrl)
                .WithAutomaticReconnect()
                .Build();
            _subscription = _hubConnection.On<PushNotification>("Send", OnSend);
            _hubConnection.Reconnecting += OnReconnecting;
            _hubConnection.Reconnected += OnReconnected;
            _hubConnection.Closed += OnClosed;
            await _hubConnection.StartAsync(stoppingToken);
            
            _log.LogInformation($"{nameof(PushNotificationHandler)}: connected to {GetLogData()}");
            _telemetryClient.TrackEvent("PushNotificationsHubConnected", GetEventData());
        }

        protected virtual async Task OnSend(PushNotification notification)
        {
            if (notification.ServerId != ScalablePushNotificationManager.ServerId)
            {
                await _storage.SavePushNotificationAsync(notification);

                _log.LogInformation($"{nameof(PushNotificationManager)}: saved push notification with {notification.Id} ID of type {notification.NotifyType} " +
                                    $"received from server {notification.ServerId} on {ScalablePushNotificationManager.ServerId}");
            }
        }

        private Task OnReconnecting(Exception arg)
        {
            _log.LogInformation($"{nameof(PushNotificationHandler)}: attempt reconnecting to {GetLogData()}");
            _telemetryClient.TrackEvent("PushNotificationsHubReconnecting", GetEventData());

            return Task.CompletedTask;
        }

        private Task OnReconnected(string arg)
        {
            _log.LogInformation($"{nameof(PushNotificationHandler)}: successfully connected to {GetLogData()}");
            _telemetryClient.TrackEvent("PushNotificationsHubReconnected", GetEventData());

            return Task.CompletedTask;
        }

        private Task OnClosed(Exception arg)
        {
            var message = $"{nameof(PushNotificationHandler)}: failed connection to {GetLogData()}";

            if (_stoppingOrDisposing)
            {
                _log.LogInformation(message);
            }
            else
            {
                _log.LogError(message);
            }

            _telemetryClient.TrackEvent("PushNotificationsHubConnectionFailed", GetEventData());

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation($"{nameof(PushNotificationHandler)}: attempt disconnecting to {GetLogData()}");
            _telemetryClient.TrackEvent("PushNotificationsHubDisconnecting", GetEventData());

            _stoppingOrDisposing = true;

            _subscription.Dispose();
            _hubConnection.Closed -= OnClosed;
            _hubConnection.Reconnected -= OnReconnected;
            _hubConnection.Reconnecting -= OnReconnecting;
            _hubConnection.StopAsync(cancellationToken);
            return base.StopAsync(cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            _stoppingOrDisposing = true;

            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
        }

        public override void Dispose()
        {
            DisposeAsync().GetAwaiter().GetResult();
            base.Dispose();
        }

        private string GetLogData()
        {
            return $"{_pushNotificationsOptions.Scalability.HubUrl} hub with {ScalablePushNotificationManager.ServerId} server ID and {_hubConnection.ConnectionId} connection ID";
        }

        private IDictionary<string, string> GetEventData()
        {
            return new Dictionary<string, string>
            {
                {"hubUrl", _pushNotificationsOptions.Scalability.HubUrl},
                {"serverId", ScalablePushNotificationManager.ServerId},
                {"connectionId", _hubConnection.ConnectionId}
            };
        }
    }
}

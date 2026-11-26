using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Web.PushNotifications.Scalability;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    public static class SignalRBuilderExtensions
    {
        public static ISignalRServerBuilder AddPushNotifications(this ISignalRServerBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddSingleton<IUserIdProvider, PushNotificationUserIdProvider>();

            var scalabilityMode = configuration["PushNotifications:ScalabilityMode"];
            var isScaledOut = scalabilityMode != null && !scalabilityMode.EqualsIgnoreCase("None");

            // Scaled out with Redis: all instances share the notifications in Redis and keep them across restarts.
            // A single instance (ScalabilityMode None), or no Redis: the instance keeps them in memory. Both apply PushNotificationStorageRules.
            var useRedisStorage = isScaledOut && !string.IsNullOrEmpty(configuration.GetConnectionString("RedisConnectionString"));
            if (useRedisStorage)
            {
                builder.Services.AddSingleton<IPushNotificationStorage, RedisPushNotificationStorage>();
            }
            else
            {
                builder.Services.AddSingleton<IPushNotificationStorage, PushNotificationInMemoryStorage>();
            }

            builder.Services.AddOptions<PushNotificationOptions>().Bind(configuration.GetSection("PushNotifications")).ValidateDataAnnotations();

            // SignalR scalability configuration.
            if (isScaledOut)
            {

                // Store full type information in JSON to deserialize push notifications on other instances
                builder.AddNewtonsoftJsonProtocol(jsonOptions =>
                {
                    jsonOptions.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
                    jsonOptions.PayloadSerializerSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
                });

                builder.Services.AddSingleton<IPushNotificationManager, ScalablePushNotificationManager>();

                // Legacy: copy other instances' notifications into the in-memory storage through the hub (PushNotifications:HubUrl with an api key).
                // Not needed with the Redis storage.
                if (!useRedisStorage && !string.IsNullOrEmpty(configuration["PushNotifications:HubUrl"]))
                {
                    builder.Services.AddHostedService<PushNotificationSynchronizerTask>();
                }

                if (scalabilityMode.EqualsIgnoreCase("AzureSignalRService"))
                {
                    var azureSignalRConnectionString = configuration["PushNotifications:AzureSignalRService:ConnectionString"];
                    if (string.IsNullOrEmpty(azureSignalRConnectionString))
                    {
                        throw new InvalidOperationException("PushNotifications:AzureSignalRService:ConnectionString must be set");
                    }
                    builder.AddAzureSignalR(o =>
                    {
                        o.Endpoints = [new ServiceEndpoint(azureSignalRConnectionString)];
                    });
                }
                else
                {
                    var redisConnectionString = configuration.GetConnectionString("RedisConnectionString");
                    if (string.IsNullOrEmpty(redisConnectionString))
                    {
                        throw new InvalidOperationException("RedisConnectionString must be set");
                    }

                    var redisChannelName = configuration["PushNotifications:RedisBackplane:ChannelName"].EmptyToNull() ?? "VirtoCommerceChannel";
                    builder.AddStackExchangeRedis(redisConnectionString, o => o.Configuration.ChannelPrefix = RedisChannel.Literal(redisChannelName));
                }
            }
            else
            {
                builder.Services.AddSingleton<IPushNotificationManager, PushNotificationManager>();
            }

            return builder;
        }
    }
}

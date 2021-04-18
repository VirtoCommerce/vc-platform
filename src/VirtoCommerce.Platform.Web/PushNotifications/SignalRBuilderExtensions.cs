using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Web.PushNotifications.Scalability;

namespace VirtoCommerce.Platform.Web.PushNotifications
{
    public static class SignalRBuilderExtensions
    {
        public static ISignalRServerBuilder AddPushNotifications(this ISignalRServerBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddSingleton<IPushNotificationStorage, PushNotificationInMemoryStorage>();

            var scalabilityMode = configuration["PushNotifications:ScalabilityMode"];

            builder.Services.AddOptions<PushNotificationOptions>().Bind(configuration.GetSection("PushNotifications")).ValidateDataAnnotations();

            // SignalR scalability configuration.
            if (scalabilityMode != null && !scalabilityMode.EqualsInvariant("None"))
            {

                //Enable to store in the json the full type information to be able deserialize a push notifications types on the other instances
                builder.AddNewtonsoftJsonProtocol(jsonOptions =>
                {
                    jsonOptions.PayloadSerializerSettings.TypeNameHandling = TypeNameHandling.Objects;
                    jsonOptions.PayloadSerializerSettings.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
                });

                builder.Services.AddSingleton<IPushNotificationManager, ScalablePushNotificationManager>();
                builder.Services.AddHostedService<PushNotificationSynchronizerTask>();

                if (scalabilityMode.EqualsInvariant("AzureSignalRService"))
                {
                    var azureSignalRConnectionString = configuration["PushNotifications:AzureSignalRService:ConnectionString"];
                    if (string.IsNullOrEmpty(azureSignalRConnectionString))
                    {
                        throw new InvalidOperationException($"PushNotifications:AzureSignalRService:ConnectionString must be set");
                    }
                    builder.AddAzureSignalR(o =>
                    {
                        o.Endpoints = new[] { new ServiceEndpoint(azureSignalRConnectionString) };
                    });
                }
                else
                {
                    var redisConnectionString = configuration.GetConnectionString("RedisConnectionString");
                    var redisChannelName = configuration["PushNotifications:RedisBackplane:ChannelName"];
                    redisChannelName = string.IsNullOrEmpty(redisChannelName) ? "VirtoCommerceChannel" : redisChannelName;
                    if (string.IsNullOrEmpty(redisConnectionString))
                    {
                        throw new InvalidOperationException($"RedisConnectionString must be set");
                    }
                    builder.AddStackExchangeRedis(redisConnectionString, o => o.Configuration.ChannelPrefix = redisChannelName);
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

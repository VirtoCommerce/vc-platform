
using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Web.SignalR
{
    public static class SignalRServerBuilderExtensions
    {
        public static void AddAzureSignalR(this ISignalRServerBuilder builder, IConfiguration configuration)
        {
            var azureSignalRConnectionString = configuration["SignalR:AzureSignalRService:ConnectionString"];
            if (string.IsNullOrEmpty(azureSignalRConnectionString))
            {
                throw new InvalidOperationException("SignalR:AzureSignalRService:ConnectionString  must be set");
            }
            builder.AddAzureSignalR(options =>
            {
                options.Endpoints = new ServiceEndpoint[] { new ServiceEndpoint(azureSignalRConnectionString) };
            });
        }

        public static void AddRedisBackplane(this ISignalRServerBuilder builder, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("RedisConnectionString");
            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                builder.AddStackExchangeRedis(redisConnectionString, options => options.Configuration.ChannelPrefix = configuration["SignalR:RedisBackplane:ChannelName"] ?? "VirtoCommerceChannel");
            }
        }
    }
}

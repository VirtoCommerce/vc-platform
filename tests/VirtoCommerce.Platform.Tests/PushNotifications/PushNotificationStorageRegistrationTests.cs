using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Web.PushNotifications;
using VirtoCommerce.Platform.Web.PushNotifications.Scalability;
using Xunit;

namespace VirtoCommerce.Platform.Tests.PushNotifications
{
    [Trait("Category", "Unit")]
    public class PushNotificationStorageRegistrationTests
    {
        private const string Redis = "127.0.0.1:6379";
        private const string HubUrl = "https://localhost/pushNotificationHub?api_key=key";

        [Theory]
        [InlineData(null, null, null, typeof(PushNotificationInMemoryStorage))]
        [InlineData("None", null, null, typeof(PushNotificationInMemoryStorage))]
        [InlineData("None", Redis, null, typeof(PushNotificationInMemoryStorage))]
        [InlineData("RedisBackplane", Redis, null, typeof(RedisPushNotificationStorage))]
        [InlineData("RedisBackplane", Redis, HubUrl, typeof(RedisPushNotificationStorage))]
        [InlineData("AzureSignalRService", Redis, null, typeof(RedisPushNotificationStorage))]
        [InlineData("AzureSignalRService", null, HubUrl, typeof(PushNotificationInMemoryStorage))]
        [InlineData("AzureSignalRService", null, null, typeof(PushNotificationInMemoryStorage))]
        public void Storage_DependsOnScaleOutAndRedis(string scalabilityMode, string redis, string hubUrl, System.Type expected)
        {
            var services = AddPushNotifications(scalabilityMode, redis, hubUrl);

            services.Single(x => x.ServiceType == typeof(IPushNotificationStorage)).ImplementationType.Should().Be(expected);
        }

        [Theory]
        [InlineData("AzureSignalRService", null, HubUrl, true)]
        [InlineData("AzureSignalRService", null, null, false)]
        [InlineData("AzureSignalRService", Redis, HubUrl, false)]
        [InlineData("RedisBackplane", Redis, HubUrl, false)]
        [InlineData("None", null, HubUrl, false)]
        public void LegacyHubSynchronizer_OnlyForScaledOutInMemoryStorageWithHubUrl(string scalabilityMode, string redis, string hubUrl, bool expected)
        {
            var services = AddPushNotifications(scalabilityMode, redis, hubUrl);

            services.Any(x => x.ServiceType == typeof(IHostedService) && x.ImplementationType == typeof(PushNotificationSynchronizerTask))
                .Should().Be(expected);
        }

        private static ServiceCollection AddPushNotifications(string scalabilityMode, string redis, string hubUrl)
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["PushNotifications:ScalabilityMode"] = scalabilityMode,
                    ["PushNotifications:HubUrl"] = hubUrl,
                    ["PushNotifications:AzureSignalRService:ConnectionString"] = "Endpoint=https://test.service.signalr.net;AccessKey=a2V5;Version=1.0;",
                    ["ConnectionStrings:RedisConnectionString"] = redis,
                })
                .Build();

            var services = new ServiceCollection();
            services.AddSignalR().AddPushNotifications(configuration);

            return services;
        }
    }
}

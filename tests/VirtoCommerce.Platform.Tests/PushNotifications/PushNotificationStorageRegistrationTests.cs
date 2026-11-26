using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Data.PushNotifications;
using VirtoCommerce.Platform.Web.PushNotifications;
using Xunit;

namespace VirtoCommerce.Platform.Tests.PushNotifications
{
    [Trait("Category", "Unit")]
    public class PushNotificationStorageRegistrationTests
    {
        private const string Redis = "127.0.0.1:6379";
        private const string Azure = "Endpoint=https://test.service.signalr.net;AccessKey=a2V5;Version=1.0;";

        [Theory]
        [InlineData(null, null, typeof(PushNotificationInMemoryStorage), null)]
        [InlineData(Redis, null, typeof(RedisPushNotificationStorage), "RedisHubLifetimeManager")]
        [InlineData(Redis, Azure, typeof(RedisPushNotificationStorage), "ServiceLifetimeManager")]
        [InlineData(null, Azure, typeof(PushNotificationInMemoryStorage), "ServiceLifetimeManager")]
        [InlineData(Redis, "Endpoint=https://{app name}.service.signalr.net;AccessKey={access key};Version=1.0;", typeof(RedisPushNotificationStorage), "RedisHubLifetimeManager")]
        [InlineData(null, "Endpoint=https://{app name}.service.signalr.net;AccessKey={access key};Version=1.0;", typeof(PushNotificationInMemoryStorage), null)]
        public void Mode_FollowsConfiguredConnectionStrings(string redis, string azure, System.Type expectedStorage, string expectedLifetimeManager)
        {
            var services = AddPushNotifications(new()
            {
                ["ConnectionStrings:RedisConnectionString"] = redis,
                ["PushNotifications:AzureSignalRService:ConnectionString"] = azure,
            });

            services.Single(x => x.ServiceType == typeof(IPushNotificationStorage)).ImplementationType.Should().Be(expectedStorage);
            services.Single(x => x.ServiceType == typeof(IPushNotificationManager)).ImplementationType.Should().Be<PushNotificationManager>();

            var lifetimeManager = services.Last(x => x.ServiceType == typeof(HubLifetimeManager<>)).ImplementationType.Name;
            if (expectedLifetimeManager == null)
            {
                lifetimeManager.Should().StartWith("DefaultHubLifetimeManager");
            }
            else
            {
                lifetimeManager.Should().StartWith(expectedLifetimeManager);
            }
        }

        [Fact]
        public void ObsoleteScalabilityModeAndHubUrl_AreIgnored()
        {
            var services = AddPushNotifications(new()
            {
                ["PushNotifications:ScalabilityMode"] = "RedisBackplane",
                ["PushNotifications:HubUrl"] = "https://localhost/pushNotificationHub?api_key=key",
            });

            services.Single(x => x.ServiceType == typeof(IPushNotificationStorage)).ImplementationType.Should().Be<PushNotificationInMemoryStorage>();
            services.Should().NotContain(x => x.ServiceType == typeof(IHostedService));
            services.Last(x => x.ServiceType == typeof(HubLifetimeManager<>)).ImplementationType.Name.Should().StartWith("DefaultHubLifetimeManager");
        }

        private static ServiceCollection AddPushNotifications(Dictionary<string, string> settings)
        {
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();

            var services = new ServiceCollection();
            services.AddSignalR().AddPushNotifications(configuration);

            return services;
        }
    }
}

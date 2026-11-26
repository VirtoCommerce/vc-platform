using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Data.PushNotifications;
using Xunit;

namespace VirtoCommerce.Platform.Tests.PushNotifications;

/// <summary>
/// The default interface methods that keep implementations built against an earlier version working.
/// </summary>
[Trait("Category", "Unit")]
public class PushNotificationInterfaceDefaultsTests
{
    [Fact]
    public async Task Storage_WithOnlySynchronousSearch_ServesTheAsynchronousMethods()
    {
        IPushNotificationStorage storage = new LegacyStorage();
        await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "a"));
        await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "b") { IsNew = false });

        var found = await storage.SearchPushNotificationsAsync("admin", new PushNotificationSearchCriteria { Take = 10 }, TestContext.Current.CancellationToken);
        var marked = await storage.MarkAllAsReadAsync("admin", TestContext.Current.CancellationToken);

        found.TotalCount.Should().Be(2);
        marked.Select(x => x.Title).Should().Equal("a");
        (await storage.SearchPushNotificationsAsync("admin", new PushNotificationSearchCriteria { OnlyNew = true }, TestContext.Current.CancellationToken)).TotalCount.Should().Be(0);
    }

    [Fact]
    public void Storage_ObsoleteSynchronousMethods_CallTheAsynchronousOnes()
    {
        IPushNotificationStorage storage = new PushNotificationInMemoryStorage(Options.Create(new PushNotificationOptions()));

#pragma warning disable VC0016 // The obsolete methods are what this test covers
        storage.SavePushNotification(new StorageTestNotification("admin", "a"));
        var found = storage.SearchPushNotifications("admin", new PushNotificationSearchCriteria { Take = 10 });
#pragma warning restore VC0016

        found.NotifyEvents.Select(x => x.Title).Should().Equal("a");
    }

    [Fact]
    public async Task Manager_WithOnlySynchronousSearch_ServesSearchAndMarkAllAsRead()
    {
        var manager = new LegacyManager();
        manager.Stored.Add(new StorageTestNotification("admin", "a"));
        IPushNotificationManager contract = manager;

        var found = await contract.SearchNotifiesAsync("admin", new PushNotificationSearchCriteria { Take = 10 }, TestContext.Current.CancellationToken);
        var marked = await contract.MarkAllAsReadAsync("admin", TestContext.Current.CancellationToken);

        found.TotalCount.Should().Be(1);
        marked.Should().ContainSingle().Which.IsNew.Should().BeFalse();
        manager.Sent.Should().ContainSingle("the default implementation sends each marked notification again");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void RedisStorage_EmptyChannelName_UsesTheDefaultPrefix(string channelName)
    {
        var options = new PushNotificationOptions { RedisBackplane = new PushNotificationRedisOptions { ChannelName = channelName } };

        var storage = new RedisPushNotificationStorage(Mock.Of<IConnectionMultiplexer>(), Options.Create(options), NullLogger<RedisPushNotificationStorage>.Instance);

        storage.KeyPrefix.Should().Be("VirtoCommerceChannel:PushNotifications");
    }

    private sealed class LegacyStorage : IPushNotificationStorage
    {
        private readonly List<PushNotification> _stored = [];

        public Task SavePushNotificationAsync(PushNotification notification)
        {
            _stored.RemoveAll(x => x.Id == notification.Id);
            _stored.Add(notification);
            return Task.CompletedTask;
        }

        public PushNotificationSearchResult SearchPushNotifications(string userId, PushNotificationSearchCriteria criteria)
        {
            return PushNotificationSearch.Search(_stored.Where(x => x.Creator == userId), criteria);
        }
    }

    private sealed class LegacyManager : IPushNotificationManager
    {
        public List<PushNotification> Stored { get; } = [];

        public List<PushNotification> Sent { get; } = [];

        public void Send(PushNotification notification) => Sent.Add(notification);

        public Task SendAsync(PushNotification notification)
        {
            Sent.Add(notification);
            return Task.CompletedTask;
        }

        public PushNotificationSearchResult SearchNotifies(string userId, PushNotificationSearchCriteria criteria)
        {
            return PushNotificationSearch.Search(Stored.Where(x => x.Creator == userId), criteria);
        }
    }
}

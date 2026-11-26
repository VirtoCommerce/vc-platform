using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Data.PushNotifications;
using Xunit;

namespace VirtoCommerce.Platform.Tests.PushNotifications;

/// <summary>
/// Runs <see cref="RedisPushNotificationStorage"/> against a real Redis: the storage rules shared with the in-memory storage, and what only Redis does.
/// Each storage has its own connection, as separate platform instances would.
/// Redis: <c>VC_TEST_REDIS_CONNECTION_STRING</c>, default <c>127.0.0.1:6379,ssl=False</c>. Tests are skipped when Redis is not reachable.
/// Every test uses its own key prefix and deletes its keys.
/// </summary>
[Trait("Category", "IntegrationTest")]
public sealed class RedisPushNotificationStorageIntegrationTests : PushNotificationStorageContractTests, IAsyncLifetime
{
    private static readonly string ConnectionString =
        Environment.GetEnvironmentVariable("VC_TEST_REDIS_CONNECTION_STRING") ?? "127.0.0.1:6379,ssl=False";

    private readonly string _channelName = $"PushNotificationTests-{Guid.NewGuid():N}";
    private readonly List<ConnectionMultiplexer> _connections = [];

    public async ValueTask InitializeAsync()
    {
        var options = ConfigurationOptions.Parse(ConnectionString);
        options.AbortOnConnectFail = false;
        options.ConnectTimeout = 2000;

        var connection = await ConnectionMultiplexer.ConnectAsync(options);
        _connections.Add(connection);
        Assert.SkipUnless(connection.IsConnected, $"Redis is not reachable at '{ConnectionString}'. Set VC_TEST_REDIS_CONNECTION_STRING to run these tests.");
    }

    public async ValueTask DisposeAsync()
    {
        var connection = _connections[0];
        if (connection.IsConnected)
        {
            var db = connection.GetDatabase();
            foreach (var server in connection.GetServers())
            {
                foreach (var key in server.Keys(db.Database, $"{_channelName}:*"))
                {
                    await db.KeyDeleteAsync(key);
                }
            }
        }

        foreach (var item in _connections)
        {
            await item.DisposeAsync();
        }
    }

    protected override IPushNotificationStorage CreateStorage(int maxPerUser = 20, TimeSpan? retention = null)
    {
        var connection = ConnectionMultiplexer.Connect(ConnectionString);
        _connections.Add(connection);

        var options = new PushNotificationOptions
        {
            MaxNotificationsPerUser = maxPerUser,
            RetentionPeriod = retention ?? TimeSpan.FromHours(24),
            RedisBackplane = new PushNotificationRedisOptions { ChannelName = _channelName },
        };

        return new RedisPushNotificationStorage(connection, Options.Create(options), NullLogger<RedisPushNotificationStorage>.Instance);
    }

    [Fact]
    public async Task Notification_SavedOnOneInstance_IsSeenOnAnotherAndAfterRestart()
    {
        var notification = new StorageTestNotification("admin", "Indexation") { ProcessedCount = 7 };

        await CreateStorage().SavePushNotificationAsync(notification);

        var found = Search(CreateStorage(), "admin").NotifyEvents.Should().ContainSingle().Subject;
        found.Should().BeOfType<StorageTestNotification>().Which.Should().BeEquivalentTo(notification, x => x.Excluding(n => n.Created));
        found.Created.Should().BeCloseTo(notification.Created, TimeSpan.FromMilliseconds(1));
    }

    [Fact]
    public async Task Save_TrimsTheHashAndSetsKeyLifetime()
    {
        var storage = CreateStorage(maxPerUser: 3);
        await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "old") { Created = DateTime.UtcNow.AddHours(-25) });
        for (var i = 0; i < 5; i++)
        {
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", $"n{i}") { Created = DateTime.UtcNow.AddSeconds(i) });
        }

        (await Database.SortedSetLengthAsync(Key("admin", "index"))).Should().Be(3);
        (await Database.HashLengthAsync(Key("admin", "data"))).Should().Be(3, "removed notifications are deleted from the hash too");
        (await Database.KeyTimeToLiveAsync(Key("admin", "index"))).Should().BeCloseTo(TimeSpan.FromHours(24), TimeSpan.FromMinutes(1));
        (await Database.KeyTimeToLiveAsync(Key("admin", "data"))).Should().BeCloseTo(TimeSpan.FromHours(24), TimeSpan.FromMinutes(1));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Notification_WithoutCreator_KeepsBothKeysUnderOneHashTag(string creator)
    {
        var storage = CreateStorage();
        await storage.SavePushNotificationAsync(new StorageTestNotification(creator, "system"));

        Search(storage, creator).NotifyEvents.Select(x => x.Title).Should().Equal("system");
        (await Database.KeyExistsAsync(Key("(none)", "index"))).Should().BeTrue();
        (await Database.KeyExistsAsync(Key("(none)", "data"))).Should().BeTrue();
    }

    [Fact]
    public async Task Search_SkipsEntriesThatCannotBeRead()
    {
        var storage = CreateStorage();
        await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "good"));
        await Database.SortedSetAddAsync(Key("admin", "index"), "bad", DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        await Database.HashSetAsync(Key("admin", "data"), "bad", """{"Type":"System.Uri","Assembly":"System.Private.Uri","Notification":"{}"}""");

        Search(storage, "admin").NotifyEvents.Select(x => x.Title).Should().Equal("good");
    }

    private IDatabase Database => _connections[0].GetDatabase();

    private RedisKey Key(string creator, string suffix) => $"{_channelName}:PushNotifications:{{{creator}}}:{suffix}";
}

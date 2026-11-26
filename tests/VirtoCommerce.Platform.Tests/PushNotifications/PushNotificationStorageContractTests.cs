using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Platform.Data.PushNotifications;
using Xunit;

namespace VirtoCommerce.Platform.Tests.PushNotifications
{
    /// <summary>
    /// The <see cref="PushNotificationStorageRules"/> every storage must follow. Each storage runs these tests through its own subclass,
    /// so notifications behave the same in memory and in Redis.
    /// </summary>
    public abstract class PushNotificationStorageContractTests
    {
        protected abstract IPushNotificationStorage CreateStorage(int maxPerUser = 20, TimeSpan? retention = null);

        [Fact]
        public async Task Save_MoreThanLimit_KeepsNewestOfThatUserOnly()
        {
            var storage = CreateStorage(maxPerUser: 20);
            await storage.SavePushNotificationAsync(new StorageTestNotification("manager", "other"));
            for (var i = 0; i < 25; i++)
            {
                await storage.SavePushNotificationAsync(new StorageTestNotification("admin", $"n{i}") { Created = DateTime.UtcNow.AddSeconds(i) });
            }

            var admin = Search(storage, "admin");
            admin.TotalCount.Should().Be(20);
            admin.NotifyEvents.Select(x => x.Title).Should().BeEquivalentTo(Enumerable.Range(5, 20).Select(i => $"n{i}"));
            Search(storage, "manager").TotalCount.Should().Be(1);
        }

        [Fact]
        public async Task Notifications_AreSeparatedByUser()
        {
            var storage = CreateStorage();
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "a"));
            await storage.SavePushNotificationAsync(new StorageTestNotification("manager", "m"));

            Search(storage, "admin").NotifyEvents.Select(x => x.Title).Should().Equal("a");
            Search(storage, "manager").NotifyEvents.Select(x => x.Title).Should().Equal("m");
            Search(storage, "nobody").TotalCount.Should().Be(0);
        }

        [Fact]
        public async Task Save_RemovesNotificationsOlderThanRetention()
        {
            var storage = CreateStorage(retention: TimeSpan.FromHours(24));
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "old") { Created = DateTime.UtcNow.AddHours(-25) });
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "fresh"));

            Search(storage, "admin").NotifyEvents.Select(x => x.Title).Should().Equal("fresh");
        }

        [Fact]
        public async Task Search_HidesNotificationsThatExpiredAfterTheLastSave()
        {
            var retention = TimeSpan.FromHours(24);
            var storage = CreateStorage(retention: retention);
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "expiring") { Created = DateTime.UtcNow - retention + TimeSpan.FromMilliseconds(200) });
            Search(storage, "admin").TotalCount.Should().Be(1);

            await Task.Delay(400, TestContext.Current.CancellationToken);

            var result = Search(storage, "admin");
            result.TotalCount.Should().Be(0);
            result.NewCount.Should().Be(0);
        }

        [Fact]
        public async Task Save_SameId_ReplacesTheNotification()
        {
            var storage = CreateStorage(maxPerUser: 2);
            var progress = new StorageTestNotification("admin", "Export") { ProcessedCount = 1 };
            await storage.SavePushNotificationAsync(progress);

            var update = new StorageTestNotification("admin", "Export") { Id = progress.Id, Created = progress.Created, ProcessedCount = 100 };
            await storage.SavePushNotificationAsync(update);
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "other") { Created = DateTime.UtcNow.AddSeconds(1) });

            var notifications = Search(storage, "admin").NotifyEvents;
            notifications.Select(x => x.Title).Should().BeEquivalentTo("Export", "other");
            notifications.OfType<StorageTestNotification>().Single(x => x.Id == progress.Id).ProcessedCount.Should().Be(100);
        }

        [Fact]
        public async Task Save_RepeatOfSameUsersLastNotification_IncreasesRepeatCount()
        {
            var storage = CreateStorage();
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "Same") { Description = "d" });
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "Same") { Description = "d" });

            Search(storage, "admin").NotifyEvents.Should().ContainSingle().Which.RepeatCount.Should().Be(1);
        }

        [Fact]
        public async Task Save_SameContentFromAnotherUser_IsNotCountedAsRepeat()
        {
            var storage = CreateStorage();
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "Same") { Description = "d" });
            await storage.SavePushNotificationAsync(new StorageTestNotification("manager", "Same") { Description = "d" });

            Search(storage, "admin").NotifyEvents.Should().ContainSingle().Which.RepeatCount.Should().Be(0);
            Search(storage, "manager").NotifyEvents.Should().ContainSingle().Which.RepeatCount.Should().Be(0);
        }

        [Fact]
        public async Task Save_ZeroLimitsKeepEverything()
        {
            var storage = CreateStorage(maxPerUser: 0, retention: TimeSpan.Zero);
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "old") { Created = DateTime.UtcNow.AddDays(-30) });
            for (var i = 0; i < 30; i++)
            {
                await storage.SavePushNotificationAsync(new StorageTestNotification("admin", $"n{i}"));
            }

            Search(storage, "admin").TotalCount.Should().Be(31);
        }

        protected static PushNotificationSearchResult Search(IPushNotificationStorage storage, string user)
        {
            return storage.SearchPushNotificationsAsync(user, new PushNotificationSearchCriteria { Take = 100 }, TestContext.Current.CancellationToken).GetAwaiter().GetResult();
        }
    }

    [Trait("Category", "Unit")]
    public class PushNotificationInMemoryStorageTests : PushNotificationStorageContractTests
    {
        protected override IPushNotificationStorage CreateStorage(int maxPerUser = 20, TimeSpan? retention = null)
        {
            return new PushNotificationInMemoryStorage(Options.Create(new PushNotificationOptions
            {
                MaxNotificationsPerUser = maxPerUser,
                RetentionPeriod = retention ?? TimeSpan.FromHours(24),
            }));
        }

        [Fact]
        public void DefaultOptions_Are20PerUserFor24Hours()
        {
            var options = new PushNotificationOptions();

            options.MaxNotificationsPerUser.Should().Be(20);
            options.RetentionPeriod.Should().Be(TimeSpan.FromHours(24));
        }
    }

    public class StorageTestNotification : PushNotification
    {
        public StorageTestNotification(string creator, string title) : base(creator)
        {
            Title = title;
            // Different descriptions keep the repeat count of identical consecutive notifications out of the tests that do not test it.
            Description = Guid.NewGuid().ToString();
        }

        public int ProcessedCount { get; set; }
    }
}

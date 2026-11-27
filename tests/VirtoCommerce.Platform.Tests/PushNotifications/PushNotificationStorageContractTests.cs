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
        /// <summary>
        /// The time every storage of a test sees; tests move it instead of waiting or backdating <see cref="PushNotification.Created"/>.
        /// </summary>
        protected ManualTimeProvider Clock { get; } = new(DateTimeOffset.UtcNow);

        protected abstract IPushNotificationStorage CreateStorage(int maxPerUser = 20, TimeSpan? retention = null);

        [Fact]
        public async Task Save_MoreThanLimit_KeepsTheMostRecentlySavedOfThatUserOnly()
        {
            var storage = CreateStorage(maxPerUser: 20);
            await storage.SavePushNotificationAsync(new StorageTestNotification("manager", "other"));
            for (var i = 0; i < 25; i++)
            {
                Clock.Advance(TimeSpan.FromSeconds(1));
                await storage.SavePushNotificationAsync(new StorageTestNotification("admin", $"n{i}"));
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
        public async Task Save_RemovesNotificationsNotSavedWithinRetention()
        {
            var storage = CreateStorage(retention: TimeSpan.FromHours(24));
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "old"));
            Clock.Advance(TimeSpan.FromHours(25));
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "fresh"));

            Search(storage, "admin").NotifyEvents.Select(x => x.Title).Should().Equal("fresh");
        }

        [Fact]
        public async Task Search_HidesNotificationsThatExpiredAfterTheLastSave()
        {
            var storage = CreateStorage(retention: TimeSpan.FromHours(24));
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "expiring"));
            Clock.Advance(TimeSpan.FromHours(23));
            Search(storage, "admin").TotalCount.Should().Be(1);

            Clock.Advance(TimeSpan.FromHours(2));

            var result = Search(storage, "admin");
            result.TotalCount.Should().Be(0);
            result.NewCount.Should().Be(0);
        }

        [Fact]
        public async Task Job_OvertakenByNewerNotifications_StaysWhileItReportsAndAfterItEnds()
        {
            var storage = CreateStorage(maxPerUser: 2);
            var job = new StorageTestNotification("admin", "Import") { ProcessedCount = 1 };
            await storage.SavePushNotificationAsync(job);
            Clock.Advance(TimeSpan.FromMinutes(1));
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "a"));
            Clock.Advance(TimeSpan.FromMinutes(1));
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "b"));

            Clock.Advance(TimeSpan.FromMinutes(1));
            job.ProcessedCount = 50;
            await storage.SavePushNotificationAsync(job);
            Search(storage, "admin").NotifyEvents.Select(x => x.Title).Should().Contain("Import", "a progress report keeps the job in the history");

            Clock.Advance(TimeSpan.FromMinutes(1));
            job.ProcessedCount = 100;
            job.Description = "Failed";
            await storage.SavePushNotificationAsync(job);

            var stored = Search(storage, "admin").NotifyEvents.OfType<StorageTestNotification>().Single(x => x.Id == job.Id);
            stored.ProcessedCount.Should().Be(100);
            stored.Description.Should().Be("Failed");
            stored.Created.Should().BeCloseTo(job.Created, TimeSpan.FromMilliseconds(1), "Created stays the start of the job");
        }

        [Fact]
        public async Task Job_RunningLongerThanRetention_StaysWhileItReports()
        {
            var storage = CreateStorage(retention: TimeSpan.FromHours(24));
            var job = new StorageTestNotification("admin", "Reindex") { ProcessedCount = 1 };
            await storage.SavePushNotificationAsync(job);

            Clock.Advance(TimeSpan.FromHours(20));
            job.ProcessedCount = 2;
            await storage.SavePushNotificationAsync(job);
            Clock.Advance(TimeSpan.FromHours(20));

            Search(storage, "admin").NotifyEvents.Should().ContainSingle()
                .Which.Should().BeOfType<StorageTestNotification>().Which.ProcessedCount.Should().Be(2);
        }

        [Fact]
        public async Task Save_SameId_ReplacesTheNotification()
        {
            var storage = CreateStorage(maxPerUser: 2);
            var progress = new StorageTestNotification("admin", "Export") { ProcessedCount = 1 };
            await storage.SavePushNotificationAsync(progress);

            Clock.Advance(TimeSpan.FromSeconds(1));
            var update = new StorageTestNotification("admin", "Export") { Id = progress.Id, Created = progress.Created, ProcessedCount = 100 };
            await storage.SavePushNotificationAsync(update);
            Clock.Advance(TimeSpan.FromSeconds(1));
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "other"));

            var notifications = Search(storage, "admin").NotifyEvents;
            notifications.Select(x => x.Title).Should().BeEquivalentTo("Export", "other");
            notifications.OfType<StorageTestNotification>().Single(x => x.Id == progress.Id).ProcessedCount.Should().Be(100);
        }

        [Fact]
        public async Task Save_SameContentTwice_KeepsBoth()
        {
            var storage = CreateStorage();
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "Same") { Description = "d" });
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "Same") { Description = "d" });

            var notifications = Search(storage, "admin").NotifyEvents;
            notifications.Should().HaveCount(2);
            notifications.Should().OnlyContain(x => x.RepeatCount == 0);
        }

        [Fact]
        public async Task MarkAllAsRead_MarksOnlyTheUsersNewNotificationsAndReturnsThem()
        {
            var storage = CreateStorage();
            var first = new StorageTestNotification("admin", "first");
            var second = new StorageTestNotification("admin", "second");
            await storage.SavePushNotificationAsync(first);
            await storage.SavePushNotificationAsync(second);
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "read") { IsNew = false });
            await storage.SavePushNotificationAsync(new StorageTestNotification("manager", "other"));

            var marked = await storage.MarkAllAsReadAsync("admin", TestContext.Current.CancellationToken);

            marked.Select(x => x.Id).Should().BeEquivalentTo([first.Id, second.Id]);
            marked.Should().OnlyContain(x => !x.IsNew);
            var admin = Search(storage, "admin");
            admin.TotalCount.Should().Be(3);
            admin.NewCount.Should().Be(0);
            Search(storage, "manager").NewCount.Should().Be(1);
            (await storage.MarkAllAsReadAsync("admin", TestContext.Current.CancellationToken)).Should().BeEmpty();
        }

        [Fact]
        public async Task Save_ZeroLimitsKeepEverything()
        {
            var storage = CreateStorage(maxPerUser: 0, retention: TimeSpan.Zero);
            await storage.SavePushNotificationAsync(new StorageTestNotification("admin", "old"));
            Clock.Advance(TimeSpan.FromDays(30));
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
            }), Clock);
        }

        [Fact]
        public void DefaultOptions_Are100PerUserFor7Days()
        {
            var options = new PushNotificationOptions();

            options.MaxNotificationsPerUser.Should().Be(100);
            options.RetentionPeriod.Should().Be(TimeSpan.FromDays(7));
        }
    }

    /// <summary>
    /// A clock that moves only when a test advances it.
    /// </summary>
    public sealed class ManualTimeProvider(DateTimeOffset now) : TimeProvider
    {
        private DateTimeOffset _now = now;

        public override DateTimeOffset GetUtcNow() => _now;

        public void Advance(TimeSpan by) => _now += by;
    }

    public class StorageTestNotification : PushNotification
    {
        public StorageTestNotification(string creator, string title) : base(creator)
        {
            Title = title;
            // A unique description per notification, so tests never depend on two notifications having the same content.
            Description = Guid.NewGuid().ToString();
        }

        public int ProcessedCount { get; set; }
    }
}

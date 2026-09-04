using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.ChangeLog;
using Xunit;

namespace VirtoCommerce.Platform.Caching.Tests
{
    [Trait("Category", "Unit")]
    [Collection(nameof(NotThreadSafeCollection))]
    public class LastChangesDeduplicationTests : MemoryCacheTestsBase, IDisposable
    {
        private class TestBaseEntity : Entity
        {
        }

        private class TestDerivedEntity : TestBaseEntity
        {
            public string Name { get; set; }
        }

        private class TestSiblingEntity : Entity
        {
        }

        // Mirrors ApplicationUser's real shape (ApplicationUser : IdentityUser, IEntity, ...): a
        // non-Entity base sits between the leaf and object, so the walk must stop at the first
        // ancestor that stops implementing IEntity, not merely before System.Object.
        private class PlainNonEntityBase
        {
        }

        private class PlainIEntity : PlainNonEntityBase, IEntity
        {
            public string Id { get; set; }
        }

        private class TestDbContext : DbContextWithTriggers
        {
            public TestDbContext(DbContextOptions options)
                : base(options)
            {
            }

            public DbSet<TestDerivedEntity> DerivedEntities { get; set; }

            public DbSet<TestSiblingEntity> SiblingEntities { get; set; }

            public DbSet<PlainIEntity> PlainEntities { get; set; }
        }

        private static readonly string[] _expectedTypeNames =
        [
            typeof(TestDerivedEntity).FullName,
            typeof(TestBaseEntity).FullName,
        ];

        private readonly LastChangesService _lastChangesService;
        private readonly LastChangesOptions _options = new();
        private readonly List<string> _cancelledTokenKeys = [];
        private readonly Action<TokenCancelledEventArgs> _originalOnTokenCancelled;
        private readonly Action<IInsertingEntry<IEntity>> _trigger;

        public LastChangesDeduplicationTests()
        {
            _lastChangesService = new LastChangesService(GetPlatformMemoryCache());

            _originalOnTokenCancelled = CancellableCacheRegion.OnTokenCancelled;
            CancellableCacheRegion.OnTokenCancelled = args => _cancelledTokenKeys.Add(args.TokenKey);

            var notifier = new Lazy<ILastChangesNotifier>(
                () => new LastChangesNotifier(_lastChangesService, Options.Create(_options)));

            // Mirrors the global trigger registered by ApplicationBuilderExtensions.UseDbTriggers.
            _trigger = entry => notifier.Value.OnEntitySaving(entry.Context, entry.Entity);
            Triggers<IEntity>.Inserting += _trigger;
        }

        public void Dispose()
        {
            Triggers<IEntity>.Inserting -= _trigger;
            CancellableCacheRegion.OnTokenCancelled = _originalOnTokenCancelled;
            GC.SuppressFinalize(this);
        }

        [Fact]
        public void SingleSaveChanges_ManyRowsOfSameType_CancelsEachTypeNameOnce()
        {
            // Arrange
            using var context = CreateContext();
            for (var i = 0; i < 10; i++)
            {
                context.DerivedEntities.Add(NewEntity($"row-{i}"));
            }

            // Act
            context.SaveChanges();

            // Assert
            Assert.Equal(_expectedTypeNames.Length, _cancelledTokenKeys.Count);
        }

        [Fact]
        public void EachSaveChanges_CancelsAgain()
        {
            // Arrange
            using var context = CreateContext();

            // Act
            for (var save = 0; save < 3; save++)
            {
                context.DerivedEntities.Add(NewEntity($"save-{save}"));
                context.SaveChanges();
            }

            // Assert
            Assert.Equal(_expectedTypeNames.Length * 3, _cancelledTokenKeys.Count);
        }

        [Fact]
        public void SingleSaveChanges_StillInvalidatesEveryTypeNameInTheChain()
        {
            // Arrange
            using var context = CreateContext();
            var initialDerived = _lastChangesService.GetLastModifiedDate(typeof(TestDerivedEntity).FullName);
            var initialBase = _lastChangesService.GetLastModifiedDate(typeof(TestBaseEntity).FullName);
            WaitForNextTick();

            // Act
            context.DerivedEntities.Add(NewEntity("row"));
            context.SaveChanges();

            // Assert
            Assert.NotEqual(initialDerived, _lastChangesService.GetLastModifiedDate(typeof(TestDerivedEntity).FullName));
            Assert.NotEqual(initialBase, _lastChangesService.GetLastModifiedDate(typeof(TestBaseEntity).FullName));
        }

        [Fact]
        public async Task SingleSaveChangesAsync_ManyRowsOfSeveralTypes_CancelsEachTypeNameOnce()
        {
            // Arrange
            using var context = CreateContext();
            for (var i = 0; i < 50; i++)
            {
                context.DerivedEntities.Add(NewEntity($"field-{i}"));
                context.SiblingEntities.Add(new TestSiblingEntity { Id = Guid.NewGuid().ToString("N") });
            }

            // Act
            await context.SaveChangesAsync(TestContext.Current.CancellationToken);

            // Assert
            Assert.Equal(3, _cancelledTokenKeys.Count);
            Assert.Contains(TokenKeyOf<TestDerivedEntity>(), _cancelledTokenKeys);
            Assert.Contains(TokenKeyOf<TestBaseEntity>(), _cancelledTokenKeys);
            Assert.Contains(TokenKeyOf<TestSiblingEntity>(), _cancelledTokenKeys);
        }

        [Fact]
        public void DeduplicationIsScopedToOneContext()
        {
            // Arrange
            using var firstContext = CreateContext();
            using var secondContext = CreateContext();

            // Act
            firstContext.DerivedEntities.Add(NewEntity("first"));
            secondContext.DerivedEntities.Add(NewEntity("second"));
            firstContext.SaveChanges();
            secondContext.SaveChanges();

            // Assert
            Assert.Equal(_expectedTypeNames.Length * 2, _cancelledTokenKeys.Count);
        }

        [Fact]
        public void IgnoredEntityType_AnnouncesNothingAtAll()
        {
            // Arrange
            _options.IgnoredEntityTypes.Add(typeof(TestDerivedEntity).FullName);
            using var context = CreateContext();

            // Act
            context.DerivedEntities.Add(NewEntity("row"));
            context.SaveChanges();

            // Assert
            Assert.Empty(_cancelledTokenKeys);
        }

        [Fact]
        public void IgnoredEntityType_DoesNotSilenceOtherEntitiesSavedWithIt()
        {
            // Arrange
            _options.IgnoredEntityTypes.Add(typeof(TestDerivedEntity).FullName);
            using var context = CreateContext();

            // Act
            context.DerivedEntities.Add(NewEntity("ignored"));
            context.SiblingEntities.Add(new TestSiblingEntity { Id = Guid.NewGuid().ToString("N") });
            context.SaveChanges();

            // Assert
            Assert.Equal([TokenKeyOf<TestSiblingEntity>()], _cancelledTokenKeys);
        }

        [Fact]
        public void IgnoredBaseType_AlsoExcludesEntitiesDerivedFromIt()
        {
            // Arrange
            _options.IgnoredEntityTypes.Add(typeof(TestBaseEntity).FullName);
            using var context = CreateContext();

            // Act
            context.DerivedEntities.Add(NewEntity("row"));
            context.SaveChanges();

            // Assert
            Assert.Empty(_cancelledTokenKeys);
        }

        [Fact]
        public void EmptyIgnoreList_TracksEverything()
        {
            // Arrange
            using var context = CreateContext();

            // Act
            context.DerivedEntities.Add(NewEntity("row"));
            context.SaveChanges();

            // Assert
            Assert.Equal(_expectedTypeNames.Length, _cancelledTokenKeys.Count);
        }

        [Fact]
        public void EntityImplementingIEntityDirectly_DoesNotAnnounceSystemObject()
        {
            // Arrange
            using var context = CreateContext();
            context.PlainEntities.Add(new PlainIEntity { Id = Guid.NewGuid().ToString("N") });

            // Act
            context.SaveChanges();

            // Assert
            Assert.Equal([TokenKeyOf<PlainIEntity>()], _cancelledTokenKeys);
        }

        [Fact]
        public void SaveChangesFailed_ClearsDeduplicationSet_SoTheNextSuccessfulSaveAnnouncesAgain()
        {
            // Arrange
            var databaseName = $"VCST-5566-{Guid.NewGuid():N}";
            var duplicateId = Guid.NewGuid().ToString("N");

            using (var seedContext = CreateContext(databaseName))
            {
                seedContext.DerivedEntities.Add(new TestDerivedEntity { Id = duplicateId, Name = "seed" });
                seedContext.SaveChanges();
            }

            _cancelledTokenKeys.Clear();

            using var context = CreateContext(databaseName);
            context.DerivedEntities.Add(new TestDerivedEntity { Id = duplicateId, Name = "duplicate" });

            // Act
            Assert.NotNull(Record.Exception(() => context.SaveChanges()));
            Assert.NotEmpty(_cancelledTokenKeys); // the failed attempt still ran the Inserting trigger

            _cancelledTokenKeys.Clear();
            context.ChangeTracker.Clear();
            context.DerivedEntities.Add(NewEntity("after-failure"));
            context.SaveChanges();

            // Assert
            Assert.Equal(_expectedTypeNames.Length, _cancelledTokenKeys.Count);
        }

        private static string TokenKeyOf<T>()
        {
            return LastChangesCacheRegion.GenerateRegionTokenKey(typeof(T).FullName);
        }

        private static TestDerivedEntity NewEntity(string name)
        {
            return new TestDerivedEntity { Id = Guid.NewGuid().ToString("N"), Name = name };
        }

        private static TestDbContext CreateContext()
        {
            return CreateContext($"VCST-5566-{Guid.NewGuid():N}");
        }

        private static TestDbContext CreateContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName)
                .Options;

            return new TestDbContext(options);
        }

        private static void WaitForNextTick()
        {
            var startTime = DateTime.Now;
            while (startTime.Equals(DateTime.Now))
            {
                System.Threading.Thread.Sleep(10);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.ChangeLog;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.ChangeLog;
using Xunit;

namespace VirtoCommerce.Platform.Caching.Tests
{
    /// <summary>
    /// VCST-5566. Every token cancellation counted here becomes one Redis backplane message in a scale-out
    /// deployment, so the count per SaveChanges is the thing under test, not just the resulting cache state.
    /// </summary>
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

        private class TestDbContext : DbContextWithTriggers
        {
            public TestDbContext(DbContextOptions options)
                : base(options)
            {
            }

            public DbSet<TestDerivedEntity> DerivedEntities { get; set; }

            public DbSet<TestSiblingEntity> SiblingEntities { get; set; }
        }

        private static readonly string[] _expectedTypeNames =
        [
            typeof(TestDerivedEntity).FullName,
            typeof(TestBaseEntity).FullName,
        ];

        private readonly ILastChangesService _lastChangesService;
        private readonly List<string> _cancelledTokenKeys = [];
        private readonly Action<TokenCancelledEventArgs> _originalOnTokenCancelled;
        private readonly Action<IInsertingEntry<IEntity>> _trigger;

        public LastChangesDeduplicationTests()
        {
            _lastChangesService = new LastChangesService(GetPlatformMemoryCache());

            _originalOnTokenCancelled = CancellableCacheRegion.OnTokenCancelled;
            CancellableCacheRegion.OnTokenCancelled = args => _cancelledTokenKeys.Add(args.TokenKey);

            var notifier = new LastChangesNotifier(_lastChangesService);

            // Mirrors the global trigger registered by ApplicationBuilderExtensions.UseDbTriggers.
            _trigger = entry => notifier.OnEntitySaving(entry.Context, entry.Entity);
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
            var options = new DbContextOptionsBuilder()
                .UseInMemoryDatabase($"VCST-5566-{Guid.NewGuid():N}")
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

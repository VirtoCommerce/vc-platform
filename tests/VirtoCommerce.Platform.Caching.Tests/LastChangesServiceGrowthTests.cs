using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Data.ChangeLog;
using Xunit;

namespace VirtoCommerce.Platform.Caching.Tests
{
    /// <summary>
    /// VCST-7077: reading the last modified date of a name must not store anything for that name,
    /// because the names come from the anonymous request body of POST /api/changes/changed-entities.
    /// </summary>
    [Trait("Category", "Unit")]
    [Collection(nameof(NotThreadSafeCollection))]
    public class LastChangesServiceGrowthTests : MemoryCacheTestsBase
    {
        [Fact]
        public void GetLastModifiedDate_UnknownNames_DoNotAddPerKeyTokens()
        {
            // Arrange
            var prefix = NewPrefix();
            var service = new LastChangesService(GetPlatformMemoryCache());

            // Act
            for (var i = 0; i < 1000; i++)
            {
                service.GetLastModifiedDate($"{prefix}{i}");
            }

            // Assert
            Assert.Equal(0, CountRegionKeyTokens(prefix));
        }

        [Fact]
        public void GetLastModifiedDate_UnknownNames_DoNotAddCacheEntries()
        {
            // Arrange
            var memoryCache = (MemoryCache)CreateCache();
            var platformMemoryCache = new PlatformMemoryCache(memoryCache, CachingOptions, new Moq.Mock<Microsoft.Extensions.Logging.ILogger<PlatformMemoryCache>>().Object);
            var service = new LastChangesService(platformMemoryCache);
            var prefix = NewPrefix();

            // Act
            for (var i = 0; i < 1000; i++)
            {
                service.GetLastModifiedDate($"{prefix}{i}");
            }

            // Assert
            Assert.Equal(0, memoryCache.Count);
        }

        [Fact]
        public void GetLastModifiedDate_UnknownNames_ReturnSameStableDate()
        {
            // Arrange
            var service = new LastChangesService(GetPlatformMemoryCache());
            var prefix = NewPrefix();

            // Act
            var first = service.GetLastModifiedDate($"{prefix}a");
            WaitForClockTick();
            var second = service.GetLastModifiedDate($"{prefix}b");
            var firstAgain = service.GetLastModifiedDate($"{prefix}a");

            // Assert
            Assert.Equal(first, second);
            Assert.Equal(first, firstAgain);
        }

        [Fact]
        public void GetLastModifiedDate_NullName_ReturnsRegionDate()
        {
            // Arrange
            var service = new LastChangesService(GetPlatformMemoryCache());

            // Act
            var nullNameDate = service.GetLastModifiedDate(null);

            // Assert
            Assert.Equal(service.GetLastModifiedDate($"{NewPrefix()}unknown"), nullNameDate);
        }

        [Fact]
        public void Reset_NullEntity_Throws()
        {
            var service = new LastChangesService(GetPlatformMemoryCache());

            Assert.Throws<ArgumentNullException>(() => service.Reset((Core.Common.IEntity)null));
        }

        [Fact]
        public void Reset_Name_ChangesOnlyThatName()
        {
            // Arrange
            var service = new LastChangesService(GetPlatformMemoryCache());
            var prefix = NewPrefix();
            var resetName = $"{prefix}reset";
            var otherName = $"{prefix}other";
            var initialReset = service.GetLastModifiedDate(resetName);
            var initialOther = service.GetLastModifiedDate(otherName);

            // Act
            WaitForClockTick();
            service.Reset(resetName);

            // Assert
            var afterReset = service.GetLastModifiedDate(resetName);
            Assert.True(afterReset > initialReset);
            Assert.Equal(initialOther, service.GetLastModifiedDate(otherName));
            Assert.Equal(afterReset, service.GetLastModifiedDate(resetName.ToUpperInvariant()));
        }

        [Fact]
        public void Reset_FromAnotherInstance_ChangesTheName()
        {
            // Arrange: a Redis message from another instance arrives as CancelForKey with the region token key
            var service = new LastChangesService(GetPlatformMemoryCache());
            var name = $"{NewPrefix()}remote";
            var initial = service.GetLastModifiedDate(name);

            // Act
            WaitForClockTick();
            CancellableCacheRegion.CancelForKey(LastChangesCacheRegion.GenerateRegionTokenKey(name), propagate: false);

            // Assert
            Assert.True(service.GetLastModifiedDate(name) > initial);
        }

        [Fact]
        public void ExpireRegion_ChangesAllNames()
        {
            // Arrange
            var service = new LastChangesService(GetPlatformMemoryCache());
            var prefix = NewPrefix();
            var resetName = $"{prefix}reset";
            var unknownName = $"{prefix}unknown";
            service.Reset(resetName);
            var initialReset = service.GetLastModifiedDate(resetName);
            var initialUnknown = service.GetLastModifiedDate(unknownName);

            // Act
            WaitForClockTick();
            LastChangesCacheRegion.ExpireRegion();

            // Assert
            Assert.True(service.GetLastModifiedDate(resetName) > initialReset);
            Assert.True(service.GetLastModifiedDate(unknownName) > initialUnknown);
        }

        [Fact]
        public void GlobalCacheReset_ChangesAllNames()
        {
            // Arrange
            var service = new LastChangesService(GetPlatformMemoryCache());
            var name = $"{NewPrefix()}unknown";
            var initial = service.GetLastModifiedDate(name);

            // Act
            WaitForClockTick();
            GlobalCacheRegion.ExpireRegion();

            // Assert
            Assert.True(service.GetLastModifiedDate(name) > initial);
        }

        [Fact]
        public void Reset_MoreNamesThanTheLimit_KeepsStoredNamesBoundedAndMovesEveryDateForward()
        {
            // Arrange
            using var service = new LastChangesService(GetPlatformMemoryCache());
            var prefix = NewPrefix();
            var unknownName = $"{prefix}unknown";
            var initialUnknown = service.GetLastModifiedDate(unknownName);
            WaitForClockTick();

            // Act
            for (var i = 0; i <= LastChangesService.MaxResetNames; i++)
            {
                service.Reset($"{prefix}{i}");
            }

            // Assert
            Assert.True(CountStoredResetNames(service) <= LastChangesService.MaxResetNames);
            Assert.True(service.GetLastModifiedDate(unknownName) > initialUnknown);
            Assert.True(service.GetLastModifiedDate($"{prefix}{LastChangesService.MaxResetNames}") > initialUnknown);
        }

        [Fact]
        public void Reset_SameNameManyTimes_StoresOneName()
        {
            // Arrange
            using var service = new LastChangesService(GetPlatformMemoryCache());
            var name = $"{NewPrefix()}same";

            // Act
            for (var i = 0; i <= LastChangesService.MaxResetNames; i++)
            {
                service.Reset(name);
            }

            // Assert
            Assert.Equal(1, CountStoredResetNames(service));
        }

        [Fact]
        public void Dispose_StopsTakingResetsFromOtherInstances()
        {
            // Arrange
            var service = new LastChangesService(GetPlatformMemoryCache());
            var name = $"{NewPrefix()}remote";
            var initial = service.GetLastModifiedDate(name);

            // Act
            service.Dispose();
            WaitForClockTick();
            CancellableCacheRegion.CancelForKey(LastChangesCacheRegion.GenerateRegionTokenKey(name), propagate: false);

            // Assert
            Assert.Equal(initial, service.GetLastModifiedDate(name));
        }

        private static int CountStoredResetNames(LastChangesService service)
        {
            var state = typeof(LastChangesService).GetField("_state", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(service);
            var resetDates = (ConcurrentDictionary<string, DateTimeOffset>)state!.GetType().GetProperty("ResetDates")!.GetValue(state);

            return resetDates!.Count;
        }

        private static string NewPrefix()
        {
            return $"VCST-7077-{Guid.NewGuid():N}-";
        }

        private static int CountRegionKeyTokens(string namePrefix)
        {
            var field = typeof(CancellableCacheRegion<LastChangesCacheRegion>)
                .GetField("_keyTokensDict", BindingFlags.NonPublic | BindingFlags.Static);
            var dictionary = (ConcurrentDictionary<string, CancellationTokenSource>)field!.GetValue(null);
            var tokenKeyPrefix = LastChangesCacheRegion.GenerateRegionTokenKey(namePrefix);

            return dictionary!.Keys.Count(x => x.StartsWith(tokenKeyPrefix, StringComparison.OrdinalIgnoreCase));
        }

        private static void WaitForClockTick()
        {
            var start = DateTimeOffset.UtcNow;
            while (DateTimeOffset.UtcNow == start)
            {
                Thread.Sleep(1);
            }
        }
    }
}

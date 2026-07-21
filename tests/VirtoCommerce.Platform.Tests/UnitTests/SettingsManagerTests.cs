using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Caching;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Settings;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    [Trait("Category", "Unit")]
    public class SettingsManagerTests
    {
        // Records the name set passed to each DB load, so a test can assert which names actually hit the DB.
        private readonly List<string[]> _loadCalls = new();

        [Fact]
        public async Task GetObjectSettingsAsync_PerNameCache_SecondOverlappingRequestLoadsOnlyMissingNames()
        {
            // Arrange
            var sut = CreateManager();
            sut.RegisterSettings([Descriptor("A"), Descriptor("B"), Descriptor("C")]);

            // Act
            var first = (await sut.GetObjectSettingsAsync(["A", "B"])).ToArray();
            var second = (await sut.GetObjectSettingsAsync(["A", "C"])).ToArray();

            // Assert — each request returns exactly its names, in order
            Assert.Equal(["A", "B"], first.Select(x => x.Name));
            Assert.Equal(["A", "C"], second.Select(x => x.Name));

            // The per-name cache means the second (overlapping) request re-loads ONLY the missing name.
            // With the previous per-name-SET cache key, "A;C" was a distinct key and BOTH A and C reloaded.
            Assert.Equal(2, _loadCalls.Count);
            Assert.Equal(["A", "B"], _loadCalls[0].OrderBy(x => x));
            Assert.Equal(["C"], _loadCalls[1]);
        }

        [Fact]
        public async Task GetObjectSettingsAsync_PreservesRequestedOrder()
        {
            var sut = CreateManager();
            sut.RegisterSettings([Descriptor("A"), Descriptor("B"), Descriptor("C")]);

            var result = (await sut.GetObjectSettingsAsync(["C", "A", "B"])).ToArray();

            Assert.Equal(["C", "A", "B"], result.Select(x => x.Name));
        }

        [Fact]
        public async Task GetObjectSettingsAsync_RequestedNameCasing_DiffersFromRegistered_ResolvesAndCachesPerName()
        {
            var sut = CreateManager();
            sut.RegisterSettings([Descriptor("Setting-A")]);

            // Request with different casing than the registered/canonical name.
            var first = (await sut.GetObjectSettingsAsync(["setting-a"])).Single();
            Assert.Equal("Setting-A", first.Name); // canonical name from the descriptor, not the request casing

            // A second differently-cased request must hit the same per-name cache entry (no reload).
            var second = (await sut.GetObjectSettingsAsync(["SETTING-A"])).Single();
            Assert.Equal("Setting-A", second.Name);
            Assert.Single(_loadCalls);
        }

        [Fact]
        public async Task GetObjectSettingsAsync_ExpireSetting_InvalidatesPerNameEntry()
        {
            var sut = CreateManager();
            sut.RegisterSettings([Descriptor("A")]);

            var first = (await sut.GetObjectSettingsAsync(["A"])).Single();
            await sut.GetObjectSettingsAsync(["A"]); // warm hit — must not reload
            Assert.Single(_loadCalls);

            // Expiring the setting's own change token is exactly what SaveObjectSettingsAsync does via ClearCache.
            SettingsCacheRegion.ExpireSetting(first);

            await sut.GetObjectSettingsAsync(["A"]);
            Assert.Equal(2, _loadCalls.Count); // per-name entry was invalidated -> reloaded
        }

        private SettingsManager CreateManager()
        {
            var repositoryMock = new Mock<IPlatformRepository>();
            repositoryMock
                .Setup(x => x.GetObjectSettingsByNamesAsync(It.IsAny<string[]>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string[] names, string objectType, string objectId) =>
                {
                    _loadCalls.Add(names);
                    var rows = names.Select(name => new SettingEntity
                    {
                        Name = name,
                        ObjectType = objectType,
                        ObjectId = objectId,
                        SettingValues = [new SettingValueEntity().SetValue(SettingValueType.ShortText, $"val-{name}")],
                    }).ToArray();

                    return Task.FromResult(rows);
                });

            var overrideProvider = new Mock<ISettingsOverrideProvider>(); // no overrides (TryGet* default to false)

            return new SettingsManager(
                () => repositoryMock.Object,
                GetPlatformMemoryCache(),
                new Mock<IEventPublisher>().Object,
                Options.Create(new FixedSettings { Settings = [] }),
                overrideProvider.Object);
        }

        private static SettingDescriptor Descriptor(string name)
        {
            return new SettingDescriptor { Name = name, ValueType = SettingValueType.ShortText };
        }

        private static PlatformMemoryCache GetPlatformMemoryCache()
        {
            var memoryCache = new MemoryCache(Options.Create(new MemoryCacheOptions()));
            return new PlatformMemoryCache(memoryCache, Options.Create(new CachingOptions()), new Mock<ILogger<PlatformMemoryCache>>().Object);
        }
    }
}

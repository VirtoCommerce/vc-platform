using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Settings;
using VirtoCommerce.Platform.Tests.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    /// <summary>
    /// VCST-6093: a VirtoCommerce:Settings:Override:DefaultValue override must reach typed reads
    /// (GetValueAsync / GetValue) when nothing is stored, not only the ObjectSettingEntry shown in the admin UI.
    /// </summary>
    [Trait("Category", "Unit")]
    public class SettingsDefaultValueOverrideTests
    {
        private const string TenantType = "ImportProfile";
        private const string TenantId = "profile-1";

        private static readonly SettingDescriptor _descriptor = new()
        {
            Name = "Test.Import.BatchSize",
            ValueType = SettingValueType.PositiveInteger,
            DefaultValue = 7,
        };

        // Stored DB rows keyed by "objectType|objectId|name"; empty means nothing is stored.
        private readonly Dictionary<string, object> _stored = new(StringComparer.OrdinalIgnoreCase);

        [Fact]
        public async Task GetObjectSettingAsync_GlobalDefaultOverride_PopulatesEntryDefault()
        {
            // The override does reach the entry — this is what the admin UI shows. Passes today.
            var sut = CreateManager(new() { ["VirtoCommerce:Settings:Override:DefaultValue:Global:Test.Import.BatchSize"] = "3" });

            var entry = await sut.GetObjectSettingAsync(_descriptor.Name);

            Assert.Null(entry.Value);
            Assert.Equal(3, entry.DefaultValue);
        }

        [Fact]
        public async Task GetValueAsync_GlobalDefaultOverride_NothingStored_ReturnsOverride()
        {
            var sut = CreateManager(new() { ["VirtoCommerce:Settings:Override:DefaultValue:Global:Test.Import.BatchSize"] = "3" });

            var result = await sut.GetValueAsync<int>(_descriptor);

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetValueAsync_GlobalDefaultOverrideInVirtoCloudForm_NothingStored_ReturnsOverride()
        {
            var sut = CreateManager(new() { ["VirtoCommerce:Settings:Override:DefaultValue:Global:Test_Import_BatchSize"] = "3" });

            var result = await sut.GetValueAsync<int>(_descriptor);

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetValueAsync_GlobalDefaultOverride_ValueStored_ReturnsStoredValue()
        {
            _stored[Key(null, null)] = 5;
            var sut = CreateManager(new() { ["VirtoCommerce:Settings:Override:DefaultValue:Global:Test.Import.BatchSize"] = "3" });

            var result = await sut.GetValueAsync<int>(_descriptor);

            Assert.Equal(5, result);
        }

        [Fact]
        public async Task ObjectSettingsGetValue_GlobalDefaultOverride_NothingStored_ReturnsOverride()
        {
            var sut = CreateManager(new() { ["VirtoCommerce:Settings:Override:DefaultValue:Global:Test.Import.BatchSize"] = "3" });
            var entity = new TestEntity { Id = TenantId };

            await sut.DeepLoadSettingsAsync(entity);
            var result = entity.Settings.GetValue<int>(_descriptor);

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task ObjectSettingsGetValue_TenantDefaultOverride_BeatsGlobal()
        {
            var sut = CreateManager(new()
            {
                ["VirtoCommerce:Settings:Override:DefaultValue:Global:Test.Import.BatchSize"] = "3",
                [$"VirtoCommerce:Settings:Override:DefaultValue:Tenants:{TenantType}:{TenantId}:Test.Import.BatchSize"] = "4",
            });
            var entity = new TestEntity { Id = TenantId };

            await sut.DeepLoadSettingsAsync(entity);
            var result = entity.Settings.GetValue<int>(_descriptor);

            Assert.Equal(4, result);
        }

        [Fact]
        public async Task ObjectSettingsGetValue_GlobalDefaultOverride_ValueStoredOnObject_ReturnsStoredValue()
        {
            _stored[Key(TenantType, TenantId)] = 5;
            var sut = CreateManager(new() { ["VirtoCommerce:Settings:Override:DefaultValue:Global:Test.Import.BatchSize"] = "3" });
            var entity = new TestEntity { Id = TenantId };

            await sut.DeepLoadSettingsAsync(entity);
            var result = entity.Settings.GetValue<int>(_descriptor);

            Assert.Equal(5, result);
        }

        private SettingsManager CreateManager(Dictionary<string, string> configuration)
        {
            var repositoryMock = new Mock<IPlatformRepository>();
            repositoryMock
                .Setup(x => x.GetObjectSettingsByNamesAsync(It.IsAny<string[]>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string[] names, string objectType, string objectId) =>
                {
                    var rows = names
                        .Where(name => _stored.ContainsKey(Key(objectType, objectId, name)))
                        .Select(name => new SettingEntity
                        {
                            Name = name,
                            ObjectType = objectType,
                            ObjectId = objectId,
                            SettingValues = [new SettingValueEntity().SetValue(_descriptor.ValueType, _stored[Key(objectType, objectId, name)])],
                        })
                        .ToArray();

                    return Task.FromResult(rows);
                });

            var overrideProvider = new ConfigurationSettingsOverrideProvider(
                new ConfigurationBuilder().AddInMemoryCollection(configuration).Build());

            var manager = new SettingsManager(
                () => repositoryMock.Object,
                MemoryCacheMockHelper.GetPlatformMemoryCache(),
                new Mock<IEventPublisher>().Object,
                Options.Create(new FixedSettings { Settings = [] }),
                overrideProvider,
                new Mock<ILogger<SettingsManager>>().Object);

            manager.RegisterSettings([_descriptor]);
            manager.RegisterSettingsForType([_descriptor], TenantType);

            return manager;
        }

        private static string Key(string objectType, string objectId, string name = null)
        {
            return $"{objectType}|{objectId}|{name ?? _descriptor.Name}";
        }

        private sealed class TestEntity : IHasSettings
        {
            public string Id { get; set; }
            public string TypeName => TenantType;
            public ICollection<ObjectSettingEntry> Settings { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Settings;
using VirtoCommerce.Platform.Tests.Common;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    [Trait("Category", "Unit")]
    public class SettingsManagerTests
    {
        // Records the name set passed to each DB load, so a test can assert which names actually hit the DB.
        private readonly List<string[]> _loadCalls = new();
        private readonly Mock<ILogger<SettingsManager>> _loggerMock = new();

        [Fact]
        public async Task GetObjectSettingsAsync_PerNameCache_SecondOverlappingRequestLoadsOnlyMissingNames()
        {
            // Arrange
            var sut = CreateManager();
            sut.RegisterSettings([Descriptor("A"), Descriptor("B"), Descriptor("C")]);

            // Act
            var first = (await sut.GetObjectSettingsAsync(["A", "B"])).ToArray();
            var second = (await sut.GetObjectSettingsAsync(["A", "C"])).ToArray();

            // Assert — each request returns exactly its names (set semantics; order not guaranteed)
            Assert.Equal(["A", "B"], first.Select(x => x.Name).OrderBy(x => x));
            Assert.Equal(["A", "C"], second.Select(x => x.Name).OrderBy(x => x));

            // The per-name cache means the second (overlapping) request re-loads ONLY the missing name.
            // With the previous per-name-SET cache key, "A;C" was a distinct key and BOTH A and C reloaded.
            Assert.Equal(2, _loadCalls.Count);
            Assert.Equal(["A", "B"], _loadCalls[0].OrderBy(x => x));
            Assert.Equal(["C"], _loadCalls[1]);
        }

        [Fact]
        public async Task GetObjectSettingsAsync_ReturnsAllRequestedSettings()
        {
            var sut = CreateManager();
            sut.RegisterSettings([Descriptor("A"), Descriptor("B"), Descriptor("C")]);

            var result = (await sut.GetObjectSettingsAsync(["C", "A", "B"])).ToArray();

            // Per-name cache has set semantics (deduped, order not guaranteed) — assert membership, not order.
            Assert.Equal(["A", "B", "C"], result.Select(x => x.Name).OrderBy(x => x));
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

        [Fact]
        public async Task GetObjectSettingAsync_StoredValueTypeDriftedFromDescriptor_CoercesToDeclaredType()
        {
            // The vcst-qa dashboard outage: descriptor declares Boolean, the persisted row says ShortText.
            // Before the fix this string reached an unguarded (bool) cast and 500'd the whole endpoint.
            var sut = CreateManager(_ => new SettingValueEntity().SetValue(SettingValueType.ShortText, "True"));
            sut.RegisterSettings([BooleanDescriptor("Order.DashboardStatistics.Enable")]);

            var setting = await sut.GetObjectSettingAsync("Order.DashboardStatistics.Enable");

            Assert.IsType<bool>(setting.Value);
            Assert.Equal(true, setting.Value);
        }

        [Fact]
        public async Task GetObjectSettingAsync_StoredValueUnconvertible_FallsBackToDescriptorDefault()
        {
            var sut = CreateManager(_ => new SettingValueEntity().SetValue(SettingValueType.ShortText, "yes"));
            sut.RegisterSettings([BooleanDescriptor("A")]);

            var setting = await sut.GetObjectSettingAsync("A");

            Assert.Equal(true, setting.Value); // the descriptor's DefaultValue, not an exception
        }

        [Fact]
        public async Task GetObjectSettingAsync_ValueTypeDrift_LogsSingleWarningPerCacheLoad()
        {
            var sut = CreateManager(_ => new SettingValueEntity().SetValue(SettingValueType.ShortText, "True"));
            sut.RegisterSettings([BooleanDescriptor("A")]);

            await sut.GetObjectSettingAsync("A");
            await sut.GetObjectSettingAsync("A"); // cache hit — must not re-warn
            await sut.GetObjectSettingAsync("A");

            Assert.Single(_loadCalls);
            Assert.Equal(1, WarningCount());
        }

        [Fact]
        public async Task GetObjectSettingAsync_StoredValueTypeMatchesDescriptor_LogsNoWarning()
        {
            var sut = CreateManager(); // default factory stores ShortText, matching the ShortText descriptor
            sut.RegisterSettings([Descriptor("A")]);

            var setting = await sut.GetObjectSettingAsync("A");

            Assert.Equal("val-A", setting.Value);
            Assert.Equal(0, WarningCount());
        }

        [Theory]
        [InlineData("True", true)]
        [InlineData("1", true)]
        [InlineData("0", false)]
        public async Task GetObjectSettingAsync_FixedSettingConfiguredAsText_ConvertsToDeclaredType(string configured, bool expected)
        {
            // Fixed settings come from appsettings.json, so a boolean arrives as text — including "1"/"0",
            // which Convert.ToBoolean rejects.
            var sut = CreateManager(fixedSettings: [FixedSetting("A", SettingValueType.Boolean, configured, defaultValue: false)]);

            var setting = await sut.GetObjectSettingAsync("A");

            Assert.IsType<bool>(setting.Value);
            Assert.Equal(expected, setting.Value);
        }

        [Fact]
        public async Task GetObjectSettingAsync_FixedSettingValueUnconvertible_FallsBackToConvertedDefault()
        {
            // The default is configured as text too, so it must be converted before it can serve as the fallback —
            // otherwise Value ends up holding the raw string.
            var sut = CreateManager(fixedSettings: [FixedSetting("A", SettingValueType.Boolean, "yes", defaultValue: "true")]);

            var setting = await sut.GetObjectSettingAsync("A");

            Assert.IsType<bool>(setting.Value);
            Assert.Equal(true, setting.Value);
            Assert.Equal(true, setting.DefaultValue);
            Assert.Equal(1, WarningCount());
        }

        [Fact]
        public async Task GetObjectSettingAsync_FixedSettingWithoutValue_KeepsEmptyValueSemantics()
        {
            // Long-standing behaviour: a missing fixed value materializes as the declared type's empty value.
            var sut = CreateManager(fixedSettings: [FixedSetting("A", SettingValueType.Boolean, value: null, defaultValue: null)]);

            var setting = await sut.GetObjectSettingAsync("A");

            Assert.Equal(false, setting.Value);
            Assert.Equal(0, WarningCount());
        }

        private SettingsManager CreateManager(Func<string, SettingValueEntity> valueFactory = null, ObjectSettingEntry[] fixedSettings = null)
        {
            valueFactory ??= name => new SettingValueEntity().SetValue(SettingValueType.ShortText, $"val-{name}");

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
                        SettingValues = [valueFactory(name)],
                    }).ToArray();

                    return Task.FromResult(rows);
                });

            var overrideProvider = new Mock<ISettingsOverrideProvider>(); // no overrides (TryGet* default to false)

            return new SettingsManager(
                () => repositoryMock.Object,
                MemoryCacheMockHelper.GetPlatformMemoryCache(),
                new Mock<IEventPublisher>().Object,
                Options.Create(new FixedSettings { Settings = fixedSettings ?? [] }),
                overrideProvider.Object,
                _loggerMock.Object);
        }

        private static SettingDescriptor Descriptor(string name)
        {
            return new SettingDescriptor { Name = name, ValueType = SettingValueType.ShortText };
        }

        private static SettingDescriptor BooleanDescriptor(string name)
        {
            return new SettingDescriptor { Name = name, ValueType = SettingValueType.Boolean, DefaultValue = true };
        }

        private static ObjectSettingEntry FixedSetting(string name, SettingValueType valueType, object value, object defaultValue)
        {
            return new ObjectSettingEntry { Name = name, ValueType = valueType, Value = value, DefaultValue = defaultValue };
        }

        private int WarningCount()
        {
            return _loggerMock.Invocations.Count(x => x.Method.Name == nameof(ILogger.Log) && (LogLevel)x.Arguments[0] == LogLevel.Warning);
        }
    }
}

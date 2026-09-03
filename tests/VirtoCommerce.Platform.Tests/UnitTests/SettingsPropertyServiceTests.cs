using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Settings;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    /// <summary>
    /// Pin behaviour of the optional <c>moduleId</c> filter on
    /// <see cref="SettingsPropertyService.GetValuesAsync"/>.
    ///
    /// Why this matters: the <c>useModuleSettings</c> composable in
    /// frontend modules (e.g. vc-module-system-operations) used to do
    /// a paired <c>/schema?moduleId=…</c> + <c>/values</c> round-trip
    /// and filter the values blob client-side to just settings owned
    /// by the requesting module. Adding the same <c>moduleId</c> filter
    /// to <c>/values</c> lets the composable drop the schema fetch
    /// entirely (one round-trip instead of two), so this test ensures
    /// both that the filter narrows correctly and that null/empty
    /// keeps the legacy "everything in scope" behaviour the existing
    /// admin Settings UI relies on.
    /// </summary>
    public class SettingsPropertyServiceTests
    {
        private const string ModuleA = "VirtoCommerce.ModuleA";
        private const string ModuleB = "VirtoCommerce.ModuleB";

        [Fact]
        public async Task GetValuesAsync_FilteredByModuleId_OnlyReturnsThatModulesSettings()
        {
            // Arrange: two modules contribute settings; the service must
            // only return ModuleA's values when filtered by moduleId.
            var descriptors = new[]
            {
                NewDescriptor("VirtoCommerce.ModuleA.Foo", ModuleA, defaultValue: 1),
                NewDescriptor("VirtoCommerce.ModuleA.Bar", ModuleA, defaultValue: "x"),
                NewDescriptor("VirtoCommerce.ModuleB.Baz", ModuleB, defaultValue: true),
            };

            var service = NewService(descriptors);

            // Act
            var result = await service.GetValuesAsync(moduleId: ModuleA);

            // Assert: ModuleA only, defaults filled in (modifiedOnly=false).
            result.Keys.Should().BeEquivalentTo(
                "VirtoCommerce.ModuleA.Foo",
                "VirtoCommerce.ModuleA.Bar");
            result["VirtoCommerce.ModuleA.Foo"].Should().Be(1);
            result["VirtoCommerce.ModuleA.Bar"].Should().Be("x");
        }

        [Fact]
        public async Task GetValuesAsync_NullModuleId_ReturnsAllAsBefore()
        {
            // Regression guard: existing callers (admin Settings UI,
            // SettingsManager.SaveValuesAsync's replaceAll diff) pass no
            // moduleId and rely on the full-scope listing. Adding the
            // optional parameter must not change that.
            var descriptors = new[]
            {
                NewDescriptor("VirtoCommerce.ModuleA.Foo", ModuleA, defaultValue: 1),
                NewDescriptor("VirtoCommerce.ModuleB.Baz", ModuleB, defaultValue: true),
            };

            var service = NewService(descriptors);

            var result = await service.GetValuesAsync();

            result.Keys.Should().BeEquivalentTo(
                "VirtoCommerce.ModuleA.Foo",
                "VirtoCommerce.ModuleB.Baz");
        }

        [Fact]
        public async Task GetValuesAsync_EmptyModuleId_ReturnsAllAsBefore()
        {
            // An empty string from a query-param shortcut should behave
            // identically to null — i.e. "no filter", not "match the empty
            // string moduleId" which would return nothing.
            var descriptors = new[]
            {
                NewDescriptor("VirtoCommerce.ModuleA.Foo", ModuleA, defaultValue: 1),
                NewDescriptor("VirtoCommerce.ModuleB.Baz", ModuleB, defaultValue: true),
            };

            var service = NewService(descriptors);

            var result = await service.GetValuesAsync(moduleId: string.Empty);

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetValuesAsync_FilteredByModuleId_UnknownModule_ReturnsEmpty()
        {
            // A moduleId that doesn't match any registered descriptor must
            // return an empty dictionary, not throw and not fall back to
            // "everything". The composable treats an empty result as "this
            // module has no settings yet" — which is the correct UX.
            var descriptors = new[]
            {
                NewDescriptor("VirtoCommerce.ModuleA.Foo", ModuleA, defaultValue: 1),
            };

            var service = NewService(descriptors);

            var result = await service.GetValuesAsync(moduleId: "VirtoCommerce.DoesNotExist");

            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetValuesAsync_DictionarySetting_ReturnsAllowedValuesArray()
        {
            var descriptors = new[]
            {
                NewDictionaryDescriptor("VirtoCommerce.ModuleA.Roles", ModuleA, "role-a", "role-b"),
            };

            var service = NewService(descriptors);

            var result = await service.GetValuesAsync();

            result["VirtoCommerce.ModuleA.Roles"].Should().BeEquivalentTo(new object[] { "role-a", "role-b" });
        }

        [Fact]
        public async Task GetValuesAsync_DictionarySetting_ModifiedOnly_ExcludesUnchangedFromDefault()
        {
            var descriptors = new[]
            {
                NewDictionaryDescriptor("VirtoCommerce.ModuleA.Roles", ModuleA, "role-a", "role-b"),
            };

            var service = NewService(descriptors);

            var result = await service.GetValuesAsync(modifiedOnly: true);

            result.Should().NotContainKey("VirtoCommerce.ModuleA.Roles");
        }

        [Fact]
        public async Task GetValuesAsync_DictionarySetting_ModifiedOnly_ExcludesEquivalentButDistinctArrayInstance()
        {
            var descriptors = new[]
            {
                NewDictionaryDescriptor("VirtoCommerce.ModuleA.Roles", ModuleA, "role-a", "role-b"),
            };

            var service = NewService(
                descriptors,
                allowedValueOverrides: new Dictionary<string, object[]> { ["VirtoCommerce.ModuleA.Roles"] = ["role-b", "role-a"] });

            var result = await service.GetValuesAsync(modifiedOnly: true);

            result.Should().NotContainKey("VirtoCommerce.ModuleA.Roles");
        }

        [Fact]
        public async Task GetValuesAsync_DictionarySetting_ModifiedOnly_IncludesOverriddenSubset()
        {
            var descriptors = new[]
            {
                NewDictionaryDescriptor("VirtoCommerce.ModuleA.Roles", ModuleA, "role-a", "role-b"),
            };

            var service = NewService(
                descriptors,
                allowedValueOverrides: new Dictionary<string, object[]> { ["VirtoCommerce.ModuleA.Roles"] = ["role-a"] });

            var result = await service.GetValuesAsync(modifiedOnly: true);

            result["VirtoCommerce.ModuleA.Roles"].Should().BeEquivalentTo(new object[] { "role-a" });
        }

        [Fact]
        public async Task SaveValuesAsync_DictionarySetting_SavesAsAllowedValues_NotValue()
        {
            var descriptors = new[]
            {
                NewDictionaryDescriptor("VirtoCommerce.ModuleA.Roles", ModuleA, "role-a", "role-b"),
            };

            List<ObjectSettingEntry> savedEntries = null;
            var service = NewService(descriptors, onSave: entries => savedEntries = entries.ToList());

            await service.SaveValuesAsync(new Dictionary<string, object>
            {
                ["VirtoCommerce.ModuleA.Roles"] = new object[] { "role-a" },
            });

            savedEntries.Should().ContainSingle();
            savedEntries[0].AllowedValues.Should().BeEquivalentTo(new object[] { "role-a" });
            savedEntries[0].Value.Should().BeNull();
        }

        // --- helpers ---

        private static SettingDescriptor NewDescriptor(string name, string moduleId, object defaultValue) => new()
        {
            Name = name,
            ModuleId = moduleId,
            ValueType = defaultValue switch
            {
                int => SettingValueType.Integer,
                bool => SettingValueType.Boolean,
                _ => SettingValueType.ShortText,
            },
            DefaultValue = defaultValue,
            GroupName = $"{moduleId}|Default",
        };

        private static SettingDescriptor NewDictionaryDescriptor(string name, string moduleId, params object[] allowedValues) => new()
        {
            Name = name,
            ModuleId = moduleId,
            ValueType = SettingValueType.ShortText,
            IsDictionary = true,
            AllowedValues = allowedValues,
            GroupName = $"{moduleId}|Default",
        };

        /// <summary>
        /// Build a <see cref="SettingsPropertyService"/> with the given
        /// descriptors as the registered universe. The settings manager
        /// returns the descriptors unmodified for any <c>GetObjectSettingsAsync</c>
        /// call (i.e. nothing persisted, all values come from defaults),
        /// which is the behaviour we need to assert the moduleId filter
        /// against without standing up a database.
        /// </summary>
        /// <param name="allowedValueOverrides">
        /// Per-setting-name AllowedValues to return instead of the descriptor's own default —
        /// simulates a saved per-object override for a dictionary setting.
        /// </param>
        /// <param name="onSave">Captures the entries passed to SaveObjectSettingsAsync, if set.</param>
        private static SettingsPropertyService NewService(
            IEnumerable<SettingDescriptor> descriptors,
            Dictionary<string, object[]> allowedValueOverrides = null,
            Action<IEnumerable<ObjectSettingEntry>> onSave = null)
        {
            var all = descriptors.ToList();

            var settingsManager = new Mock<ISettingsManager>();
            settingsManager.SetupGet(x => x.AllRegisteredSettings).Returns(all);
            settingsManager.Setup(x => x.GetSettingTypeAssignments())
                .Returns(new Dictionary<string, string[]>());
            settingsManager.Setup(x => x.GetSettingsForType(It.IsAny<string>()))
                .Returns(Enumerable.Empty<SettingDescriptor>());

            // Materialise descriptors as ObjectSettingEntry copies — Value
            // unset, so SettingsPropertyService.GetValuesAsync falls back
            // to DefaultValue (the path our tests exercise), unless an
            // AllowedValues override was supplied for that setting name.
            settingsManager
                .Setup(x => x.GetObjectSettingsAsync(
                    It.IsAny<IEnumerable<string>>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .ReturnsAsync((IEnumerable<string> names, string _, string __) =>
                    names.Select(n =>
                    {
                        var d = all.First(x => x.Name == n);
                        var entry = new ObjectSettingEntry(d);
                        if (allowedValueOverrides != null && allowedValueOverrides.TryGetValue(n, out var overrideValues))
                        {
                            entry.AllowedValues = overrideValues;
                        }
                        return entry;
                    }).ToList());

            settingsManager
                .Setup(x => x.SaveObjectSettingsAsync(It.IsAny<IEnumerable<ObjectSettingEntry>>()))
                .Callback<IEnumerable<ObjectSettingEntry>>(entries => onSave?.Invoke(entries.ToList()))
                .Returns(Task.CompletedTask);

            var overrideProvider = new Mock<ISettingsOverrideProvider>();
            object _ = null;
            overrideProvider
                .Setup(x => x.TryGetCurrentValue(
                    It.IsAny<SettingDescriptor>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    out _))
                .Returns(false);

            return new SettingsPropertyService(settingsManager.Object, overrideProvider.Object);
        }
    }
}

using System;
using System.Threading.Tasks;
using Moq;
using VirtoCommerce.Platform.Core.Settings;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    [Trait("Category", "Unit")]
    public class SettingsExtensionTests
    {
        private static readonly SettingDescriptor _descriptor = new()
        {
            Name = "Order.DashboardStatistics.Enable",
            ValueType = SettingValueType.Boolean,
            DefaultValue = true,
        };

        private static readonly SettingDescriptor _integerDescriptor = new()
        {
            Name = "Test.Import.BatchSize",
            ValueType = SettingValueType.PositiveInteger,
            DefaultValue = 7,
        };

        [Fact]
        public async Task GetValueAsync_ValueIsStringForBooleanSetting_ConvertsInsteadOfThrowing()
        {
            // Reproduces the reported 500: "Unable to cast object of type 'System.String' to type 'System.Boolean'".
            var manager = CreateManager("False");

            var result = await manager.GetValueAsync<bool>(_descriptor);

            Assert.False(result);
        }

        [Fact]
        public async Task GetValueAsync_ValueCannotBeConverted_ReturnsDescriptorDefault()
        {
            var manager = CreateManager("yes");

            var result = await manager.GetValueAsync<bool>(_descriptor);

            Assert.True(result);
        }

        [Fact]
        public async Task GetValueAsync_ValueIsNull_ReturnsDescriptorDefault()
        {
            var manager = CreateManager(null);

            var result = await manager.GetValueAsync<bool>(_descriptor);

            Assert.True(result);
        }

        [Fact]
        public async Task GetValueAsync_ValueAlreadyTyped_ReturnsStoredValue()
        {
            var manager = CreateManager(false);

            var result = await manager.GetValueAsync<bool>(_descriptor);

            Assert.False(result);
        }

        // VCST-6093: a DefaultValue override from configuration lands in ObjectSettingEntry.DefaultValue
        // (Value stays null when nothing is stored). Typed reads must honour it before the descriptor's compiled-in default.

        [Fact]
        public async Task GetValueAsync_ValueIsNull_EntryDefaultOverridden_ReturnsEntryDefault()
        {
            var manager = CreateManager(_integerDescriptor, new ObjectSettingEntry(_integerDescriptor) { DefaultValue = 3 });

            var result = await manager.GetValueAsync<int>(_integerDescriptor);

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetValueAsync_ValueStored_EntryDefaultOverridden_ReturnsStoredValue()
        {
            var manager = CreateManager(_integerDescriptor, new ObjectSettingEntry(_integerDescriptor) { Value = 5, DefaultValue = 3 });

            var result = await manager.GetValueAsync<int>(_integerDescriptor);

            Assert.Equal(5, result);
        }

        [Fact]
        public async Task GetValueAsync_ValueCannotBeConverted_EntryDefaultOverridden_ReturnsEntryDefault()
        {
            var manager = CreateManager(_integerDescriptor, new ObjectSettingEntry(_integerDescriptor) { Value = "many", DefaultValue = 3 });

            var result = await manager.GetValueAsync<int>(_integerDescriptor);

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task GetValueAsync_ValueIsNull_EntryDefaultCannotBeConverted_ReturnsDescriptorDefault()
        {
            // A DefaultValue override of a dictionary setting is an array of allowed values, not a scalar.
            var manager = CreateManager(_integerDescriptor, new ObjectSettingEntry(_integerDescriptor) { DefaultValue = new object[] { 3, 4 } });

            var result = await manager.GetValueAsync<int>(_integerDescriptor);

            Assert.Equal(7, result);
        }

        [Fact]
        public void ObjectSettingsGetValue_ValueIsNull_EntryDefaultCannotBeConverted_ReturnsDescriptorDefault()
        {
            ObjectSettingEntry[] settings = [new ObjectSettingEntry(_integerDescriptor) { DefaultValue = new object[] { 3, 4 } }];

            var result = settings.GetValue<int>(_integerDescriptor);

            Assert.Equal(7, result);
        }

        [Fact]
        public void ObjectSettingsGetValue_ValueIsNull_EntryDefaultOverridden_ReturnsEntryDefault()
        {
            ObjectSettingEntry[] settings = [new ObjectSettingEntry(_integerDescriptor) { DefaultValue = 3 }];

            var result = settings.GetValue<int>(_integerDescriptor);

            Assert.Equal(3, result);
        }

        [Fact]
        public void ObjectSettingsGetValue_ValueStored_EntryDefaultOverridden_ReturnsStoredValue()
        {
            ObjectSettingEntry[] settings = [new ObjectSettingEntry(_integerDescriptor) { Value = 5, DefaultValue = 3 }];

            var result = settings.GetValue<int>(_integerDescriptor);

            Assert.Equal(5, result);
        }

        [Fact]
        public void ObjectSettingsGetValue_SettingNotLoaded_ReturnsDescriptorDefault()
        {
            var result = Array.Empty<ObjectSettingEntry>().GetValue<int>(_integerDescriptor);

            Assert.Equal(7, result);
        }

        private static ISettingsManager CreateManager(object value)
        {
            return CreateManager(_descriptor, new ObjectSettingEntry(_descriptor) { Value = value });
        }

        private static ISettingsManager CreateManager(SettingDescriptor descriptor, ObjectSettingEntry entry)
        {
            var managerMock = new Mock<ISettingsManager>();
            managerMock
                .Setup(x => x.GetObjectSettingAsync(descriptor.Name, null, null))
                .ReturnsAsync(entry);

            return managerMock.Object;
        }
    }
}

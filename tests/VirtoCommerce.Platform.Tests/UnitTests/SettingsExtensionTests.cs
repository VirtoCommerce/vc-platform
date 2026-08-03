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

        private static ISettingsManager CreateManager(object value)
        {
            var managerMock = new Mock<ISettingsManager>();
            managerMock
                .Setup(x => x.GetObjectSettingAsync(_descriptor.Name, null, null))
                .ReturnsAsync(new ObjectSettingEntry(_descriptor) { Value = value });

            return managerMock.Object;
        }
    }
}

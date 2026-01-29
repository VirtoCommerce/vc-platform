using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Settings;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    public class ConfigurationSettingsOverrideProviderTests
    {
        [Fact]
        public void TryGetForced_ShouldReadDictionaryArrayValuesFromConfiguration()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["VirtoCommerce:Settings:Override:CurrentValue:Global:Platform.TestSetting:0"] = "1",
                    ["VirtoCommerce:Settings:Override:CurrentValue:Global:Platform.TestSetting:1"] = "2",
                })
                .Build();

            var descriptor = new SettingDescriptor
            {
                Name = "Platform.TestSetting",
                IsDictionary = true,
                ValueType = SettingValueType.Integer,
            };

            var provider = new ConfigurationSettingsOverrideProvider(configuration);

            // Act
            var found = provider.TryGetCurrentValue(descriptor, objectType: null, objectId: null, out var value);

            // Assert
            found.Should().BeTrue();
            value.Should().BeOfType<object[]>();
            var array = (object[])value;
            array.Should().Equal(1, 2);
        }

        [Fact]
        public void TryGetForced_ShouldIgnoreScalarOverrideWhenFormatIsComplex()
        {
            // Arrange
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["VirtoCommerce:Settings:Override:CurrentValue:Global:Platform.SendDiagnosticData:0"] = "true",
                })
                .Build();

            var descriptor = new SettingDescriptor
            {
                Name = "Platform.SendDiagnosticData",
                ValueType = SettingValueType.Boolean,
            };

            var provider = new ConfigurationSettingsOverrideProvider(configuration);

            // Act
            var found = provider.TryGetCurrentValue(descriptor, objectType: null, objectId: null, out var value);

            // Assert
            found.Should().BeFalse();
            value.Should().BeNull();
        }
    }
}

using System;
using System.Globalization;
using System.Threading;
using VirtoCommerce.Platform.Core.Settings;
using Xunit;

namespace VirtoCommerce.Platform.Tests.UnitTests
{
    [Trait("Category", "Unit")]
    public class SettingValueConverterTests
    {
        [Theory]
        [InlineData("True", true)]
        [InlineData("true", true)]
        [InlineData("  true  ", true)]
        [InlineData("False", false)]
        [InlineData("1", true)]
        [InlineData("0", false)]
        public void TryConvert_StringToBoolean_Converts(string stored, bool expected)
        {
            var success = SettingValueConverter.TryConvert(stored, SettingValueType.Boolean, out var result);

            Assert.True(success);
            Assert.IsType<bool>(result);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(SettingValueType.Integer, "42", 42)]
        [InlineData(SettingValueType.PositiveInteger, "42", 42)]
        public void TryConvert_StringToInteger_Converts(SettingValueType valueType, string stored, int expected)
        {
            var success = SettingValueConverter.TryConvert(stored, valueType, out var result);

            Assert.True(success);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TryConvert_StringToDecimal_UsesInvariantCulture()
        {
            // A comma-decimal ambient culture would parse "1.5" as 15 without the invariant culture.
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
            try
            {
                var success = SettingValueConverter.TryConvert("1.5", SettingValueType.Decimal, out var result);

                Assert.True(success);
                Assert.Equal(1.5m, result);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }

        [Fact]
        public void TryConvert_StringToDateTime_Converts()
        {
            var success = SettingValueConverter.TryConvert("2026-07-29T10:30:00", SettingValueType.DateTime, out var result);

            Assert.True(success);
            Assert.Equal(new DateTime(2026, 7, 29, 10, 30, 0), result);
        }

        [Fact]
        public void TryConvert_BooleanToString_Converts()
        {
            var success = SettingValueConverter.TryConvert(true, SettingValueType.ShortText, out var result);

            Assert.True(success);
            Assert.Equal("True", result);
        }

        [Theory]
        [InlineData(SettingValueType.Boolean)]
        [InlineData(SettingValueType.DateTime)]
        [InlineData(SettingValueType.Integer)]
        [InlineData(SettingValueType.ShortText)]
        public void TryConvert_Null_StaysNull(SettingValueType valueType)
        {
            // Converting null would silently invent false / DateTime.MinValue / 0 / string.Empty.
            var success = SettingValueConverter.TryConvert(null, valueType, out var result);

            Assert.True(success);
            Assert.Null(result);
        }

        [Fact]
        public void TryConvert_AlreadyCorrectType_ReturnsValueUnchanged()
        {
            var stored = "already text";

            var success = SettingValueConverter.TryConvert(stored, SettingValueType.ShortText, out var result);

            Assert.True(success);
            Assert.Same(stored, result);
        }

        [Theory]
        [InlineData("yes", SettingValueType.Boolean)]
        [InlineData("abc", SettingValueType.Integer)]
        [InlineData("abc", SettingValueType.Decimal)]
        [InlineData("not a date", SettingValueType.DateTime)]
        public void TryConvert_Unconvertible_ReturnsFalseAndDoesNotThrow(string stored, SettingValueType valueType)
        {
            var success = SettingValueConverter.TryConvert(stored, valueType, out var result);

            Assert.False(success);
            Assert.Null(result);
        }

        [Fact]
        public void TryConvertGeneric_StringToBoolean_Converts()
        {
            var success = SettingValueConverter.TryConvert<bool>("True", out var result);

            Assert.True(success);
            Assert.True(result);
        }

        [Fact]
        public void TryConvertGeneric_NullableTargets_Converts()
        {
            Assert.True(SettingValueConverter.TryConvert<bool?>("true", out var boolean));
            Assert.Equal(true, boolean);

            Assert.True(SettingValueConverter.TryConvert<DateTime?>("2026-07-29", out var dateTime));
            Assert.Equal(new DateTime(2026, 7, 29), dateTime);
        }

        [Fact]
        public void TryConvertGeneric_Unconvertible_ReturnsFalseWithDefault()
        {
            var success = SettingValueConverter.TryConvert<bool>("yes", out var result);

            Assert.False(success);
            Assert.False(result);
        }

        [Fact]
        public void TryConvertGeneric_Null_ReturnsFalse()
        {
            var success = SettingValueConverter.TryConvert<bool>(null, out var result);

            Assert.False(success);
            Assert.False(result);
        }
    }
}

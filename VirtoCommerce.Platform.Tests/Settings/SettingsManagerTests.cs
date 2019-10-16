using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using CacheManager.Core;
using Moq;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Model;
using VirtoCommerce.Platform.Data.Repositories;
using VirtoCommerce.Platform.Data.Settings;
using Xunit;

namespace VirtoCommerce.Platform.Tests.Settings
{
    [Trait("Category", "Unit")]
    [CLSCompliant(false)]
    public class SettingsManagerTests
    {
        private const string _settingName = "TestSetting";

        public static object[][] DecimalValues = {
            //             env    appStn DB    def   expected
            new object[] { "1.1", "2.2", 3.3m, 4.4m, 1.1m },
            new object[] { "1.1", "2.2", 3.3m, null, 1.1m },
            new object[] { null,  "2.2", 3.3m, 4.4m, 2.2m },
            new object[] { null,  "2.2", 3.3m, null, 2.2m },
            new object[] { null,  null,  3.3m, 4.4m, 3.3m },
            new object[] { null,  null,  3.3m, null, 3.3m },
            new object[] { null,  null,  null, 4.4m, 4.4m },
            new object[] { null,  null,  null, null, null },
        };

        public static object[][] DateTimeValues = {
            //             environment appSettings database                  default                   expected
            new object[] { "2019-1-1", "2019-1-2", new DateTime(2019, 1, 3), new DateTime(2019, 1, 4), new DateTime(2019, 1, 1) },
            new object[] { "2019-1-1", "2019-1-2", new DateTime(2019, 1, 3), null,                     new DateTime(2019, 1, 1) },
            new object[] { null,       "2019-1-2", new DateTime(2019, 1, 3), new DateTime(2019, 1, 4), new DateTime(2019, 1, 2) },
            new object[] { null,       "2019-1-2", new DateTime(2019, 1, 3), null,                     new DateTime(2019, 1, 2) },
            new object[] { null,       null,       new DateTime(2019, 1, 3), new DateTime(2019, 1, 4), new DateTime(2019, 1, 3) },
            new object[] { null,       null,       new DateTime(2019, 1, 3), null,                     new DateTime(2019, 1, 3) },
            new object[] { null,       null,       null,                     new DateTime(2019, 1, 4), new DateTime(2019, 1, 4) },
            new object[] { null,       null,       null,                     null,                     null                     },
        };

        [Theory]
        [InlineData("environment", "appSettings", "database", "default", "environment")]
        [InlineData("", "appSettings", "database", "default", "appSettings")] // Environment variable value cannot be empty
        [InlineData(null, "appSettings", "database", "default", "appSettings")]
        [InlineData(null, "", "database", "default", "")]
        [InlineData(null, null, "database", "default", "database")]
        [InlineData(null, null, "", "default", "")]
        [InlineData(null, null, null, "default", "default")]
        [InlineData(null, null, null, null, null)]
        public void GetStringValue(string environmentValue, string appSettingsValue, string databaseValue, string defaultValue, string expectedValue)
        {
            var repository = PrepareEnvironment(_settingName, environmentValue, appSettingsValue, databaseValue);
            var settingsManager = GetSettingsManager(repository);

            var value = settingsManager.GetValue(_settingName, defaultValue);

            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [InlineData("1", "2", 3, 4, 1, false)]
        [InlineData("1", "2", 3, null, 1, false)]
        [InlineData(null, "2", 3, 4, 2, false)]
        [InlineData(null, "2", 3, null, 2, false)]
        [InlineData(null, null, 3, 4, 3, false)]
        [InlineData(null, null, 3, null, 3, false)]
        [InlineData(null, "", 3, 4, 3, true)] // FormatException
        [InlineData(null, "", 3, null, null, false)] // No exception
        [InlineData(null, null, null, 4, 4, false)]
        [InlineData(null, null, null, null, null, false)]
        public void GetIntegerValue(string environmentValue, string appSettingsValue, int? databaseValue, int? defaultValue, int? expectedValue, bool expectedException)
        {
            var repository = PrepareEnvironment(_settingName, environmentValue, appSettingsValue, databaseValue);
            var settingsManager = GetSettingsManager(repository);

            int? GetValue()
            {
                return defaultValue == null
                    ? settingsManager.GetValue<int?>(_settingName, null)
                    : settingsManager.GetValue(_settingName, defaultValue.Value);
            }

            if (expectedException)
            {
                Assert.Throws<FormatException>(() => GetValue());
            }
            else
            {
                var value = GetValue();
                Assert.Equal(expectedValue, value);
            }
        }

        [Theory]
        [InlineData("true", "false", false, false, true)]
        [InlineData("true", "false", false, null, true)]
        [InlineData("false", "true", true, true, false)]
        [InlineData("false", "true", true, null, false)]
        [InlineData(null, "true", false, false, true)]
        [InlineData(null, "true", false, null, true)]
        [InlineData(null, "false", true, true, false)]
        [InlineData(null, "false", true, null, false)]
        [InlineData(null, null, true, false, true)]
        [InlineData(null, null, true, null, true)]
        [InlineData(null, null, false, true, false)]
        [InlineData(null, null, false, null, false)]
        [InlineData(null, null, null, false, false)]
        [InlineData(null, null, null, true, true)]
        [InlineData(null, null, null, null, null)]
        public void GetBooleanValue(string environmentValue, string appSettingsValue, bool? databaseValue, bool? defaultValue, bool? expectedValue)
        {
            var repository = PrepareEnvironment(_settingName, environmentValue, appSettingsValue, databaseValue);
            var settingsManager = GetSettingsManager(repository);

            var value = defaultValue != null
                ? settingsManager.GetValue(_settingName, defaultValue.Value)
                : settingsManager.GetValue<bool?>(_settingName, null);

            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [MemberData(nameof(DecimalValues))]
        public void GetDecimalValue(string environmentValue, string appSettingsValue, decimal? databaseValue, decimal? defaultValue, decimal? expectedValue)
        {
            var repository = PrepareEnvironment(_settingName, environmentValue, appSettingsValue, databaseValue);
            var settingsManager = GetSettingsManager(repository);

            var value = defaultValue != null
                ? settingsManager.GetValue(_settingName, defaultValue.Value)
                : settingsManager.GetValue<decimal?>(_settingName, null);

            Assert.Equal(expectedValue, value);
        }

        [Theory]
        [MemberData(nameof(DateTimeValues))]
        public void GetDateTimeValue(string environmentValue, string appSettingsValue, DateTime? databaseValue, DateTime? defaultValue, DateTime? expectedValue)
        {
            var repository = PrepareEnvironment(_settingName, environmentValue, appSettingsValue, databaseValue);
            var settingsManager = GetSettingsManager(repository);

            var value = defaultValue != null
                ? settingsManager.GetValue(_settingName, defaultValue.Value)
                : settingsManager.GetValue<DateTime?>(_settingName, null);

            Assert.Equal(expectedValue, value);
        }


        private static IPlatformRepository PrepareEnvironment(string settingName, string environmentValue, string appSettingsValue, object databaseValue)
        {
            var environmentVariableName = $"{ConfigurationHelper.AppSettingPrefix}{settingName}";
            Environment.SetEnvironmentVariable(environmentVariableName, GetNullableStringValue(environmentValue));

            ConfigurationManager.AppSettings[settingName] = GetNullableStringValue(appSettingsValue);

            return GetPlatformRepository(settingName, databaseValue);
        }

        private static string GetNullableStringValue(object value)
        {
            return value == null
                ? null
                : string.Format(CultureInfo.InvariantCulture, "{0}", value);
        }

        private static IPlatformRepository GetPlatformRepository(string settingName, object settingValue)
        {
            var valueEntity = GetSettingValueEntity(settingValue);

            var settingEntity = valueEntity == null ? null : new SettingEntity
            {
                Name = settingName,
                SettingValueType = SettingValueEntity.TypeShortText,
                SettingValues = new ObservableCollection<SettingValueEntity> { valueEntity },
            };

            var mock = new Mock<IPlatformRepository>();

            mock
                .Setup(x => x.GetSettingByName(settingName))
                .Returns(settingEntity);

            return mock.Object;
        }

        private static SettingValueEntity GetSettingValueEntity(object value)
        {
            if (value is string stringValue)
            {
                return new SettingValueEntity
                {
                    ValueType = SettingValueEntity.TypeShortText,
                    ShortTextValue = stringValue,
                };
            }

            if (value is int intValue)
            {
                return new SettingValueEntity
                {
                    ValueType = SettingValueEntity.TypeInteger,
                    IntegerValue = intValue,
                };
            }

            if (value is decimal decimalValue)
            {
                return new SettingValueEntity
                {
                    ValueType = SettingValueEntity.TypeDecimal,
                    DecimalValue = decimalValue,
                };
            }

            if (value is bool boolValue)
            {
                return new SettingValueEntity
                {
                    ValueType = SettingValueEntity.TypeBoolean,
                    BooleanValue = boolValue,
                };
            }

            if (value is DateTime dateValue)
            {
                return new SettingValueEntity
                {
                    ValueType = SettingValueEntity.TypeDateTime,
                    DateTimeValue = dateValue,
                };
            }

            return null;
        }

        private static ISettingsManager GetSettingsManager(IPlatformRepository repository)
        {
            return new SettingsManager(GetModuleCatalog(), () => repository, GetCacheManager(), null);
        }

        private static IModuleCatalog GetModuleCatalog()
        {
            return new Mock<IModuleCatalog>().Object;
        }

        private static ICacheManager<object> GetCacheManager()
        {
            return new Mock<ICacheManager<object>>().Object;
        }
    }
}

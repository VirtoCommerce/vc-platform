using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace VirtoCommerce.Platform.Core.Common
{
    public class ConfigurationHelper
    {
        public const string ConnectionStringPrefix = "VIRTO_CONN_STR_";
        public const string AppSettingPrefix = "VIRTO_APP_SETTING_";

        private static readonly char[] _valuesSeparator = { ';' };

        public static string GetNonEmptyConnectionStringValue(string nameOrConnectionString)
        {
            if (nameOrConnectionString == null)
                throw new ArgumentNullException(nameof(nameOrConnectionString));

            var result = GetConnectionStringValue(nameOrConnectionString);

            // Return original nameOrConnectionString if cannot find connection string by name
            if (string.IsNullOrEmpty(result))
            {
                result = nameOrConnectionString;
            }

            return result;
        }

        public static string GetConnectionStringValue(string nameOrConnectionString)
        {
            if (nameOrConnectionString == null)
                throw new ArgumentNullException(nameof(nameOrConnectionString));

            var result = nameOrConnectionString;

            // Find connection string by name
            if (nameOrConnectionString.IndexOf('=') < 0)
            {
                result = Environment.GetEnvironmentVariable($"{ConnectionStringPrefix}{nameOrConnectionString}");

                if (string.IsNullOrEmpty(result))
                {
                    result = ConfigurationManager.ConnectionStrings[nameOrConnectionString]?.ConnectionString;
                }
            }

            return result;
        }

        public static string[] SplitAppSettingsStringValue(string name)
        {
            return SplitAppSettingsStringValue(name, string.Empty, _valuesSeparator, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] SplitAppSettingsStringValue(string name, string defaultValue, char[] separator, StringSplitOptions options)
        {
            return GetAppSettingsValue(name, defaultValue).Split(separator, options);
        }

        public static TimeSpan GetAppSettingsValue(string name, TimeSpan defaultValue)
        {
            var stringValue = GetAppSettingsValue(name);

            var result = stringValue != null
                ? TimeSpan.Parse(stringValue, CultureInfo.InvariantCulture)
                : defaultValue;

            return result;
        }

        public static string GetAppSettingsValue(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            return GetAppSettingsValue<string>(name, null);
        }

        [CLSCompliant(false)]
        public static T GetAppSettingsValue<T>(string name, T defaultValue)
            where T : IConvertible
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var value = Environment.GetEnvironmentVariable($"{AppSettingPrefix}{name}");

            if (value == null && ConfigurationManager.AppSettings.AllKeys.Contains(name))
            {
                value = ConfigurationManager.AppSettings[name];
            }

            var result = value != null
                ? (T)Convert.ChangeType(value, typeof(T))
                : defaultValue;

            return result;
        }

        public static IList<string> GetAppSettingsNames()
        {
            var names = new List<string>(ConfigurationManager.AppSettings.AllKeys);

            var environmentVariables = Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry de in environmentVariables)
            {
                var name = (string)de.Key;
                if (name.StartsWith(AppSettingPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    name = name.Substring(AppSettingPrefix.Length);
                    names.Add(name);
                }
            }

            var result = names
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            return result;
        }
    }
}

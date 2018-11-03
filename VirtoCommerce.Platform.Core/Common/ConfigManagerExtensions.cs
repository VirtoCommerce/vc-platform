using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;

namespace VirtoCommerce.Platform.Core.Common
{
    /// <summary>
    /// http://softwarebydefault.com/code-samples/extending-configurationmanager-get-values-by-type/
    /// </summary>
    public static class ConfigManagerExtensions
    {
        private static readonly char[] _valuesSeparator = { ';' };

        /// <summary>
        /// Splits named value by ';' and returns an array of non-empty substrings
        /// </summary>
        /// <param name="nameValueCollection"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [Obsolete("Use ConfigurationHelper.SplitAppSettingsStringValue()")]
        [CLSCompliant(false)]
        public static string[] SplitStringValue(this NameValueCollection nameValueCollection, string name)
        {
            return nameValueCollection.SplitStringValue(name, string.Empty, _valuesSeparator, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Splits named value by given separator and returns an array of substrings
        /// </summary>
        /// <param name="nameValueCollection"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <param name="separator"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        [Obsolete("Use ConfigurationHelper.SplitAppSettingsStringValue()")]
        [CLSCompliant(false)]
        public static string[] SplitStringValue(this NameValueCollection nameValueCollection, string name, string defaultValue, char[] separator, StringSplitOptions options)
        {
            return nameValueCollection.GetValue(name, defaultValue).Split(separator, options);
        }

        [Obsolete("Use ConfigurationHelper.GetAppSettingsValue()")]
        [CLSCompliant(false)]
        public static T GetValue<T>(this NameValueCollection nameValueCollection, string name, T defaultValue)
            where T : IConvertible
        {
            T result;

            if (nameValueCollection.AllKeys.Contains(name))
            {
                var stringValue = nameValueCollection[name];
                result = (T)Convert.ChangeType(stringValue, typeof(T));
            }
            else
            {
                result = defaultValue;
            }

            return result;
        }

        [Obsolete("Use ConfigurationHelper.GetAppSettingsValue()")]
        public static TimeSpan GetValue(this NameValueCollection nameValueCollection, string name, TimeSpan defaultValue)
        {
            TimeSpan result;

            if (nameValueCollection.AllKeys.Contains(name))
            {
                var stringValue = nameValueCollection[name];
                result = TimeSpan.Parse(stringValue, CultureInfo.InvariantCulture);
            }
            else
            {
                result = defaultValue;
            }

            return result;
        }
    }
}

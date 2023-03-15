using System;
using System.Globalization;
using System.Linq;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class EnumUtility
    {
        public static T SafeParse<T>(string value, T defaultValue)
            where T : struct
        {
            if (!Enum.TryParse(value, out T result))
            {
                result = defaultValue;
            }

            return result;
        }

        public static T SafeParseFlags<T>(string value, T defaultValue)
            where T : struct
        {
            return SafeParseFlags(value, defaultValue, ",");
        }

        public static T SafeParseFlags<T>(string value, T defaultValue, string separator)
            where T : struct
        {
            if (!typeof(T).IsDefined(typeof(FlagsAttribute), false))
            {
                throw new ArgumentException($"{typeof(T).FullName} type should have [Flags] attribute.");
            }

            var result = 0;
            var wasAssigned = false;

            if (!string.IsNullOrEmpty(value))
            {
                var parts = value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var part in parts)
                {
                    if (Enum.TryParse(part.Trim(), true, out T partResult))
                    {
                        result |= Convert.ToInt32(partResult);
                        wasAssigned = true;
                    }
                }
            }

            return wasAssigned ? (T)Enum.ToObject(typeof(T), result) : defaultValue;
        }

        public static string SafeRemoveFlagFromEnumString<T>(string value, T flag, char separator = ',')
            where T : struct, IConvertible
        {
            if (!typeof(T).IsDefined(typeof(FlagsAttribute), false))
            {
                throw new ArgumentException($"{typeof(T).FullName} type should have [Flags] attribute.");
            }

            var result = value;

            if (!string.IsNullOrEmpty(value))
            {
                if (int.TryParse(value, out var intValue))
                {
                    intValue &= ~flag.ToInt32(CultureInfo.InvariantCulture);
                    result = intValue.ToString();
                }
                else if (Enum.TryParse(value, ignoreCase: true, out T enumValue))
                {
                    intValue = enumValue.ToInt32(CultureInfo.InvariantCulture);
                    intValue &= ~flag.ToInt32(CultureInfo.InvariantCulture);
                    result = ((T)(object)intValue).ToString();
                    result = result?.Replace(", ", separator.ToString());
                }
                else
                {
                    var parts = value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    result = string.Join(separator, parts.Where(x => !x.EqualsInvariant(flag.ToString())));
                }
            }
            return result;
        }
    }
}

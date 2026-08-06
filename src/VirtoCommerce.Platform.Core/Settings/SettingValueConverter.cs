using System;
using System.Globalization;

namespace VirtoCommerce.Platform.Core.Settings
{
    /// <summary>
    /// Converts loosely-typed setting values to the CLR type implied by the registered setting descriptor.
    /// <para>
    /// The descriptor is the authority on a setting's type; the type a value was persisted with is not.
    /// A stored value can drift away from the descriptor (e.g. a row written as <see cref="SettingValueType.ShortText"/>
    /// for a setting that is declared <see cref="SettingValueType.Boolean"/>), and a single drifted value must never
    /// take down the caller — hence every entry point here reports failure instead of throwing.
    /// </para>
    /// </summary>
    public static class SettingValueConverter
    {
        /// <summary>
        /// Converts <paramref name="value"/> to the CLR type declared by <paramref name="valueType"/>.
        /// A null value passes through as null (converting it would silently invent <c>false</c>, <c>0</c>
        /// or <see cref="DateTime.MinValue"/>).
        /// </summary>
        /// <returns>False when the value is present but cannot be converted.</returns>
        public static bool TryConvert(object value, SettingValueType valueType, out object result)
        {
            if (value == null)
            {
                result = null;
                return true;
            }

            return TryChangeType(value, GetClrType(valueType), out result);
        }

        /// <summary>
        /// Converts <paramref name="value"/> to <typeparamref name="T"/>, unwrapping nullable target types.
        /// </summary>
        /// <returns>False when the value is null or cannot be converted, leaving <paramref name="result"/> at its default.</returns>
        public static bool TryConvert<T>(object value, out T result)
        {
            result = default;

            if (value == null)
            {
                return false;
            }

            if (value is T typedValue)
            {
                result = typedValue;
                return true;
            }

            var targetType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

            if (TryChangeType(value, targetType, out var converted))
            {
                result = (T)converted;
                return true;
            }

            return false;
        }

        /// <summary>
        /// The CLR type a setting of the given <see cref="SettingValueType"/> is expected to hold.
        /// </summary>
        public static Type GetClrType(SettingValueType valueType)
        {
            return valueType switch
            {
                SettingValueType.Boolean => typeof(bool),
                SettingValueType.DateTime => typeof(DateTime),
                SettingValueType.Decimal => typeof(decimal),
                SettingValueType.Integer or SettingValueType.PositiveInteger => typeof(int),
                _ => typeof(string),
            };
        }

        private static bool TryChangeType(object value, Type targetType, out object result)
        {
            if (targetType.IsInstanceOfType(value))
            {
                result = value;
                return true;
            }

            // Convert.ToBoolean only accepts "True"/"False" from a string, so "1" and "0" — both plausible in a
            // drifted text row — would throw. Handle booleans before falling through to the generic conversion.
            if (targetType == typeof(bool) && value is string booleanText)
            {
                return TryParseBoolean(booleanText, out result);
            }

            try
            {
                result = Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
                return true;
            }
            catch (Exception ex) when (ex is InvalidCastException or FormatException or OverflowException or ArgumentException)
            {
                result = null;
                return false;
            }
        }

        private static bool TryParseBoolean(string text, out object result)
        {
            var trimmed = text.Trim();

            if (bool.TryParse(trimmed, out var parsed))
            {
                result = parsed;
                return true;
            }

            if (decimal.TryParse(trimmed, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
            {
                result = number != 0m;
                return true;
            }

            result = null;
            return false;
        }
    }
}

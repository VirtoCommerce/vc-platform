using System;
using System.Text.RegularExpressions;

namespace VirtoCommerce.ManagementClient.Localization
{
    /// <summary>
    /// Contains information about a localized value.
    /// </summary>
    public abstract class LocalizedValue
    {
        /// <summary>
        /// The localized property.
        /// </summary>
        public LocalizedProperty Property { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizedValue"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <exception cref="ArgumentNullException"><paramref name="property"/> is null.</exception>
        protected LocalizedValue(LocalizedProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            Property = property;
        }

        /// <summary>
        /// Retrieves the localized value from resources or by other means.
        /// </summary>
        /// <returns>
        /// The localized value.
        /// </returns>
        protected abstract object GetLocalizedValue();

        /// <summary>
        /// Updates the localized value.
        /// </summary>
        internal protected void UpdateValue()
        {
            Property.SetValue(GetValue());
        }

        /// <summary>
        /// Retruns the localized value.
        /// </summary>
        /// <returns>The localized value.</returns>
        internal object GetValue()
        {
            var localizedValue = GetLocalizedValue();

            var converter = Property.Converter;

            if (converter != null)
            {
                localizedValue = converter.Convert(
                    localizedValue,
                    Property.GetValueType(),
                    Property.ConverterParameter,
                    Property.GetCulture()
                    );
            }

            return localizedValue;
        }

        const string EscapeCharacters = "[][:~`'!@#$%^&*()+=/?{}.,|\\\\\"]";

        protected static string Normalize(string keyName)
        {
            return Regex.Replace(keyName, EscapeCharacters, "_");
        }
    }
}

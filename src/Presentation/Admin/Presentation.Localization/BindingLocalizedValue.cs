using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Localization
{
    /// <summary>
    /// Retrieves a localized value while getting it's Category from a binding.
    /// </summary>
    public class BindingLocalizedValue : LocalizedValue
    {
        private readonly string _category;
        private readonly string _resourceKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindingLocalizedValue"/> class.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="binding">The cateory binding.</param>
        /// <param name="path">path to take resource key</param>
        public BindingLocalizedValue(
            LocalizedDependencyProperty property,
            Binding binding,
            string path
            )
            : base(property)
        {
            CheckPropertySupported(property);

            if (binding == null)
            {
                throw new ArgumentNullException("binding");
            }

            if (binding.Converter != null)
            {
                Property.Converter = binding.Converter;
                binding.Converter = null;
            }

            // resolve resource key
            var b = new Binding(path) { Source = Property.Object.GetValue(FrameworkElement.DataContextProperty) };
            BindingOperations.SetBinding(_dummy, Dummy.ValueProperty, b);
            _resourceKey = _dummy.GetValue(Dummy.ValueProperty) as string;

            if (!string.IsNullOrEmpty(_resourceKey) && _resourceKey[0] == '^')
            {
                _resourceKey = _resourceKey.Substring(1);
                _category = LocalizationScope.DefaultCategory;
            }
            else
            {
                // resolve resource category
                if (binding.Source == null) binding.Source = b.Source;
                BindingOperations.SetBinding(_dummy, Dummy.ValueProperty, binding);
                _category = _dummy.GetValue(Dummy.ValueProperty) as string ?? LocalizationScope.DefaultCategory;
            }
        }

        /// <summary>
        /// Retrieves the localized value from resources or by other means.
        /// </summary>
        /// <returns>The localized value.</returns>
        protected override object GetLocalizedValue()
        {
            if (Property.Object == null)
            {
                return null;
            }

            object result;
#if DEBUG
            if ((bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(DependencyObject)).Metadata.DefaultValue)
            {
                result = GetFallbackValue();
            }
            else
            {
#endif
                result = _resourceKey.Localize(Normalize(_resourceKey), _category) ?? GetFallbackValue();
#if DEBUG
            }
#endif
            return result;
        }

        private static readonly Dummy _dummy = new Dummy();


        /// <summary>
        /// Returns a value when a resource is not found.
        /// </summary>
        /// <returns>
        /// "[ResourceKey]".
        /// </returns>
        string GetFallbackValue()
        {
            return "[BindingLoc Value]";
        }

        #region Internal static methods

        /// <summary>
        /// Checks if binding localization can be used on the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// 	<c>true</c> binding localization can be used; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="property"/> is null.</exception>
        internal static void CheckPropertySupported(LocalizedProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            if (property.GetValueType() == typeof(string) || property.GetValueType() == typeof(object))
            {
                // The property is supported
            }
            else
            {
                throw new InvalidOperationException("Only properties of type 'System.String' and 'System.Object' are supported.");
            }
        }

        #endregion

        private class Dummy : DependencyObject
        {
            public static readonly DependencyProperty ValueProperty =
                DependencyProperty.Register("Value", typeof(object), typeof(Dummy), new UIPropertyMetadata(null));

        }
    }
}

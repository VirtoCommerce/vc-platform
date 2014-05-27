using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Threading;

namespace VirtoCommerce.ManagementClient.Localization
{
    /// <summary>
    /// Enables localization of data-bound dependency properties.
    /// </summary>
    [ContentProperty("Bindings")]
    [MarkupExtensionReturnType(typeof(object))]
    public class LocBindingExtension : MarkupExtension
    {
        /// <summary>
        /// The resource key to use to format the values.
        /// </summary>
        /// <remarks>
        /// If both <see cref="ResourceKey"/> and <see cref="StringFormat"/> is specified
        /// <see cref="ResourceKey"/> has a priority.
        /// </remarks>
        public string ResourceKey { get; set; }

        /// <summary>
        /// The string to use to format the values.
        /// </summary>
        /// <remarks>
        /// If both <see cref="ResourceKey"/> and <see cref="StringFormat"/> is specified
        /// <see cref="ResourceKey"/> has a priority.
        /// </remarks>
        public string StringFormat { get; set; }

        Collection<BindingBase> _bindings;

        /// <summary>
        /// The bindings to pass as arguments to the format string.
        /// </summary>
        public Collection<BindingBase> Bindings
        {
            get
            {
                if (_bindings == null)
                {
                    _bindings = new Collection<BindingBase>();
                }

                return _bindings;
            }
        }

        public Binding CategoryBinding { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocBindingExtension"/> class.
        /// </summary>
        public LocBindingExtension() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocBindingExtension"/> class.
        /// </summary>
        /// <param name="resourceKey">The resource key to use to obtain the localized value.</param>
        public LocBindingExtension(string resourceKey)
        {
            ResourceKey = resourceKey;
        }

        /// <summary>
        /// When implemented in a derived class, returns an object that is set as the value of the target property for this markup extension.
        /// </summary>
        /// <param name="serviceProvider">Object that can provide services for the markup extension.</param>
        /// <returns>
        /// The object value to set on the property where the extension is applied.
        /// </returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;

            if (service == null)
            {
                return null;
            }

            if (service.TargetObject is DependencyObject)
            {
                if (service.TargetProperty is DependencyProperty)
                {
                    var property = new LocalizedDependencyProperty(
                        (DependencyObject)service.TargetObject,
                        (DependencyProperty)service.TargetProperty
                        );

                    LocalizedValue localizedValue;

                    if (CategoryBinding != null)
                    {
                        localizedValue = new BindingLocalizedValue(property, CategoryBinding, StringFormat);
                    }
                    else
                    {
                        // Check if the property supports binding localization
                        MultiBindingLocalizedValue.CheckPropertySupported(property);

                        if (string.IsNullOrEmpty(ResourceKey) && string.IsNullOrEmpty(StringFormat))
                        {
                            // Either a resource key of a format string must be specified
                            return null;
                        }

                        if (_bindings == null || _bindings.Count == 0)
                        {
                            // At least one binding must be specified
                            return null;
                        }

                        localizedValue = new MultiBindingLocalizedValue(
                            property,
                            ResourceKey,
                            StringFormat,
                            _bindings
                            );
                    }

                    LocalizationManager.InternalAddLocalizedValue(localizedValue);

                    if (property.IsInDesignMode)
                    {
                        // At design time VS designer does not set the parent of any control
                        // before its properties are set. For this reason the correct values
                        // of inherited attached properties cannot be obtained.
                        // Therefore, to display the correct localized value it must be updated
                        // later ater the parrent of the control has been set.

                        ((DependencyObject)service.TargetObject).Dispatcher.BeginInvoke(
                            new SendOrPostCallback(x => ((LocalizedValue)x).UpdateValue()),
                            DispatcherPriority.ApplicationIdle,
                            localizedValue
                            );
                    }

                    return localizedValue.GetValue();
                }
                else
                {
                    throw new Exception("This extension can be used only with dependency properties.");
                }
            }
            else if (service.TargetProperty is DependencyProperty || service.TargetProperty is PropertyInfo)
            {
                // The extension is used in a template

                return this;
            }
            else
            {
                return null;
            }
        }
    }
}

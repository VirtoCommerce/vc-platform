using System;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Threading;

namespace VirtoCommerce.ManagementClient.Localization
{
	/// <summary>
	/// Enables localization of properties.
	/// </summary>
	[ContentProperty("ResourceKey")]
	[MarkupExtensionReturnType(typeof(object))]
	public class LocExtension : MarkupExtension
	{
		/// <summary>
		/// The resource key to use to obtain the localized value.
		/// </summary>
		/// <remarks>
		/// If both <see cref="ResourceKey"/> and <see cref="Callback"/> are specified
		/// the latter has a priority.
		/// </remarks>
		public string ResourceKey { get; set; }

		/// <summary>
		/// The method to use to retrieve the localized value.
		/// </summary>
		/// <remarks>
		/// <para>
		/// If both <see cref="ResourceKey"/> and <see cref="Callback"/> are specified
		/// the latter has a priority.
		/// </para>
		/// <para>
		/// If <see cref="Converter"/> is specified it is used after the callback returns
		/// a value.
		/// </para>
		/// </remarks>
		public LocalizationCallbackReference Callback { get; set; }

		/// <summary>
		/// The parameter to pass to the callback.
		/// </summary>
		public object CallbackParameter { get; set; }

		/// <summary>
		/// The converter to use to convert the value before it is assigned to the property.
		/// </summary>
		public IValueConverter Converter { get; set; }

		/// <summary>
		/// The parameter to pass to the converter.
		/// </summary>
		public object ConverterParameter { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="LocExtension"/> class.
		/// </summary>
		public LocExtension() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="LocExtension"/> class.
		/// </summary>
		/// <param name="resourceKey">The resource key to use to obtain the localized value.</param>
		public LocExtension(string resourceKey)
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
				LocalizedProperty property;

				if (service.TargetProperty is DependencyProperty)
				{
					property = new LocalizedDependencyProperty(
						(DependencyObject)service.TargetObject,
						(DependencyProperty)service.TargetProperty
						);
				}
				else if (service.TargetProperty is PropertyInfo)
				{
					property = new LocalizedNonDependencyProperty(
						(DependencyObject)service.TargetObject,
						(PropertyInfo)service.TargetProperty
						);
				}
				else
				{
					return null;
				}

				property.Converter = Converter;

				property.ConverterParameter = ConverterParameter;

				var localizedValue = CreateLocalizedValue(property);

				if (localizedValue == null)
				{
					return null;
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

		LocalizedValue CreateLocalizedValue(LocalizedProperty property)
		{
			if (Callback != null && Callback.GetCallback() != null)
			{
				return new MethodLocalizedValue(property, Callback.GetCallback(), CallbackParameter);
			}
			else if (string.IsNullOrEmpty(ResourceKey))
			{
				return null;
			}
			else
			{
				return new ResourceLocalizedValue(property, ResourceKey);
			}
		}

		//public override string ToString()
		//{
		//	return ResourceKey;
		//}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Localization
{
	/// <summary>
	/// Retrieves a localized value from resources.
	/// </summary>
	public class MultiBindingLocalizedValue : LocalizedValue, IServiceProvider, IProvideValueTarget
	{
		string _resourceKey;

		string _stringFormat;

		IEnumerable<BindingBase> _bindings;

		/// <summary>
		/// Initializes a new instance of the <see cref="MultiBindingLocalizedValue"/> class.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <param name="resourceKey">The resource key.</param>
		/// <param name="stringFormat">The string format.</param>
		/// <param name="bindings">The bindings.</param>
		/// <exception cref="ArgumentNullException"><paramref name="property"/> is null.</exception>
		/// <exception cref="ArgumentException">The type of the <paramref name="property"/> is neither
		/// <see cref="String"/> nor <see cref="Object"/>.</exception>
		/// <exception cref="ArgumentException"><paramref name="resourceKey"/> is null or empty
		/// AND <paramref name="stringFormat"/> is null or empty.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="bindings"/> is null.</exception>
		/// <exception cref="ArgumentException"><paramref name="bindings"/> is empty.</exception>
		public MultiBindingLocalizedValue(
			LocalizedDependencyProperty property,
			string resourceKey,
			string stringFormat,
			ICollection<BindingBase> bindings
			)
			: base(property)
		{
			CheckPropertySupported(property);

			if (string.IsNullOrEmpty(resourceKey) && string.IsNullOrEmpty(stringFormat))
			{
				throw new ArgumentException("Either a resource key of a format string must be specified.");
			}

			if (bindings == null)
			{
				throw new ArgumentNullException("bindings");
			}

			if (bindings.Count == 0)
			{
				throw new ArgumentException("At least one binding must be specified.", "bindings");
			}

			_resourceKey = resourceKey;

			_stringFormat = stringFormat;

			_bindings = bindings;
		}

		/// <summary>
		/// Retrieves the localized value from resources or by other means.
		/// </summary>
		/// <returns>The localized value.</returns>
		protected override object GetLocalizedValue()
		{
			var obj = Property.Object;
			if (obj == null)
			{
				return null;
			}

			string formatString;

			if (string.IsNullOrEmpty(_resourceKey))
			{
				formatString = _stringFormat;
			}
			else
			{
#if DEBUG
				if ((bool)DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(DependencyObject)).Metadata.DefaultValue)
				{
					formatString = Normalize(_resourceKey) ?? GetFallbackValue();
				}
				else
				{
#endif
					if (_resourceKey[0] == '^')
					{
						var newResourceKey = _resourceKey.Substring(1);
						formatString = newResourceKey.Localize(Normalize(newResourceKey), LocalizationScope.DefaultCategory) ?? GetFallbackValue();
					}
					else
					{
						formatString = _resourceKey.Localize(Normalize(_resourceKey), Property.GetCategory()) ?? GetFallbackValue();
					}
#if DEBUG
				}
#endif
			}

			var binding = new MultiBinding()
			{
				StringFormat = formatString,
				Mode = BindingMode.OneWay,
				// The "MultiBinding" type internally uses the converter culture both 
				// with converters and format strings
				ConverterCulture = Property.GetCulture(),
			};

			foreach (var item in _bindings)
			{
				binding.Bindings.Add(item);
			}

			return binding.ProvideValue(this);
		}

		/// <summary>
		/// Returns a value when a resource is not found.
		/// </summary>
		/// <returns>
		/// "[ResourceKey]".
		/// </returns>
		string GetFallbackValue()
		{
			return "[" + _resourceKey + "]";
		}

		#region IServiceProvider Members

		/// <summary>
		/// Gets the service object of the specified type.
		/// </summary>
		/// <param name="serviceType">An object that specifies the type of service object to get.</param>
		/// <returns>
		/// A service object of type <paramref name="serviceType"/>.
		/// -or-
		/// null if there is no service object of type <paramref name="serviceType"/>.
		/// </returns>
		object IServiceProvider.GetService(Type serviceType)
		{
			if (serviceType == typeof(IProvideValueTarget))
			{
				return this;
			}
			else
			{
				return null;
			}
		}

		#endregion

		#region IProvideValueTarget Members

		/// <summary>
		/// Gets the target object being reported.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// The target object being reported.
		/// </returns>
		object IProvideValueTarget.TargetObject
		{
			get
			{
				return Property.Object;
			}
		}

		object IProvideValueTarget.TargetProperty
		{
			get
			{
				return (DependencyProperty)Property.Property;
			}
		}

		#endregion

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
	}
}

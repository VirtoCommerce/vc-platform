using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VirtoCommerce.Client.Globalization;

namespace VirtoCommerce.ManagementClient.Localization
{
	/// <summary>
	/// Retrieves a localized value from resources.
	/// </summary>
	public class ResourceLocalizedValue : LocalizedValue
	{
		string _resourceKey;

		/// <summary>
		/// Initializes a new instance of the <see cref="ResourceLocalizedValue"/> class.
		/// </summary>
		/// <param name="property">The property.</param>
		/// <param name="resourceKey">The resource key.</param>
		/// <exception cref="ArgumentNullException"><paramref name="property"/> is null.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="resourceKey"/> is null or empty.</exception>
		public ResourceLocalizedValue(LocalizedProperty property, string resourceKey)
			: base(property)
		{
			if (string.IsNullOrEmpty(resourceKey))
			{
				throw new ArgumentNullException("resourceKey");
			}

			_resourceKey = resourceKey;
		}

		/// <summary>
		/// Retrieves the localized value from resources or by other means.
		/// </summary>
		/// <returns>
		/// The localized value.
		/// </returns>
		protected override object GetLocalizedValue()
		{
			string value;
			if (_resourceKey[0] == '^')
			{
				var newResourceKey = _resourceKey.Substring(1);
				value = newResourceKey.Localize(Normalize(newResourceKey), LocalizationScope.DefaultCategory);
			}
			else
			{
				value = _resourceKey.Localize(Normalize(_resourceKey), Property.GetCategory() ?? LocalizationScope.DefaultCategory);
			}

			if (value == null)
			{
				return GetFallbackValue();
			}
			else if (Property.Converter != null)
			{
				return value;
			}
			else
			{
				return ChangeValueType(Property.GetValueType(), value);
			}
		}

		/// <summary>
		/// Returns a value when a resource is not found.
		/// </summary>
		/// <returns>
		/// "[ResourceKey]" if the property is of type <see cref="String"/>. Otherwise, <c>null</c>.
		/// </returns>
		object GetFallbackValue()
		{
			if (Property.GetValueType() == typeof(string) || Property.GetValueType() == typeof(object))
			{
				return "[" + _resourceKey + "]";
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Attempts to change the type of a loaded resource to the type of the property.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		/// <remarks>
		/// Supports the following property types: images, icons, text, enumerations, numbers, boolean,
		/// <see cref="DateTime"/> and a <see cref="TypeConverter"/> if one is defined for the
		/// property's type.
		/// </remarks>
		object ChangeValueType(Type type, object value)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			if (type == typeof(object))
			{
				return value;
			}
			else if (type == value.GetType() || type.IsAssignableFrom(value.GetType()))
			{
				return value;
			}
			else if (type == typeof(ImageSource))
			{
				BitmapSource result;

				if (value is Bitmap)
				{
					using ((Bitmap)value)
					{
						result = Imaging.CreateBitmapSourceFromHBitmap(((Bitmap)value).GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
					}
				}
				else if (value is Icon)
				{
					using ((Icon)value)
					{
						using (var bitmap = ((Icon)value).ToBitmap())
						{
							result = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
						}
					}
				}
				else if (value is byte[])
				{
					using (var memoryStream = new MemoryStream((byte[])value, false))
					{
						result = BitmapFrame.Create(memoryStream);
					}
				}
				else
				{
					// Return the value as is

					return value;
				}

				result.Freeze();

				return result;
			}
			else if (type.IsEnum && value is string)
			{
				return Enum.Parse(type, (string)value);
			}
			else if (value is IConvertible && (type.IsPrimitive || type == typeof(DateTime)))
			{
				return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
			}
			else
			{
				var converter = TypeDescriptor.GetConverter(type);

				if (converter == null || converter.GetType() == typeof(TypeConverter))
				{
					// No converter was found or the default converter was returned
					// (the default converter is unusable)

					if (Property.IsInDesignMode)
					{
						// VS fails to load some converters in design mode

						if (type == typeof(System.Windows.Media.Brush))
						{
							converter = new BrushConverter();

							return converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
						}
					}

					return value;
				}
				else
				{
					return converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
				}
			}
		}
	}
}

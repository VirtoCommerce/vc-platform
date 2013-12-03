using System;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Catalog.Model;

namespace VirtoCommerce.ManagementClient.Catalog.Infrastructure
{
	public class PropertyValueConverter : IValueConverter, IMultiValueConverter
	{
		private static ThreadLocal<PropertyValueConverter> _instance = new ThreadLocal<PropertyValueConverter>(() => new PropertyValueConverter());
		public static PropertyValueConverter Current
		{
			get
			{
				return _instance.Value;
			}
		}

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var parameterString = parameter as string;
			object retVal = null;

			if (parameterString == "PropertyValueType")
			{
				retVal = ((PropertyValueType)value).ToString();
			}
			else if (parameterString == "Int2PropertyValueType")
			{
				retVal = (PropertyValueType)value;
			}
			else
			{
				PropertyAndPropertyValueBase input = null;
				var item = value as PropertyValueBase;
				if (item == null)
				{
					input = value as PropertyAndPropertyValueBase;
					if (input != null)
						item = input.Value;
				}

				// decoding dictionary value
				if (input != null && input.Property != null)
				{
					var parameterProperty = input.Property;
					if (!parameterProperty.IsEnum)
					{
						item = input.Value;
					}
					else if (parameterProperty.IsMultiValue && input.Values != null)
					{
						retVal = string.Join(", ", input.Values.ToList().Select(val => parameterProperty.PropertyValues.FirstOrDefault(x => x.PropertyValueId == val.KeyValue).ToString()));
					}
					else if (parameterProperty.IsEnum)
					{
						if (input.Value != null)
							item = parameterProperty.PropertyValues.FirstOrDefault(x => x.PropertyValueId == input.Value.KeyValue);
					}
				}

				if (item != null &&
					(input == null ||
						((input.Property == null || !input.Property.IsMultiValue))))
				{
					var valueTypeHashCode = item.ValueType.GetHashCode();
					if (valueTypeHashCode == PropertyValueType.ShortString.GetHashCode())
						retVal = item.ShortTextValue;
					else if (valueTypeHashCode == PropertyValueType.LongString.GetHashCode())
						retVal = item.LongTextValue;
					else if (valueTypeHashCode == PropertyValueType.Decimal.GetHashCode())
						retVal = item.DecimalValue.ToString();
					else if (valueTypeHashCode == PropertyValueType.Integer.GetHashCode())
						retVal = item.IntegerValue.ToString();
					else if (valueTypeHashCode == PropertyValueType.Boolean.GetHashCode())
						retVal = item.BooleanValue.ToString();
					else if (valueTypeHashCode == PropertyValueType.DateTime.GetHashCode())
						if (item.DateTimeValue.HasValue)
							retVal = item.DateTimeValue.Value.ToString("d");
						//else if (valueTypeHashCode == PropertyValueType.DictionaryKey.GetHashCode())
						//    retVal = item.ShortTextValue;
						else if (valueTypeHashCode == PropertyValueType.Image.GetHashCode())
							retVal = item.ShortTextValue;
				}

				if (retVal == null)
					retVal = "N/A";
			}

			return retVal;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string parameterString = parameter as string;
			object retVal = null;

			if (parameterString == "Int2PropertyValueType")
				if (value is PropertyValueType)
					retVal = ((PropertyValueType)value).GetHashCode();
				else
					retVal = -1;

			return retVal;
		}

		#region IMultiValueConverter
		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			//object parameterObject = null;
			//if (values.Length > 0 && values[1] is Property)
			//{
			//    parameterObject = values[1];
			//}

			return Convert(values[0], targetType, parameter, culture);
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}

using System;
using System.Threading;
using System.Windows.Data;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{
	public class PropertyValueConverter : IValueConverter
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
			string parameterString = parameter as string;
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
				retVal = value == null || value.ToString() == String.Empty ? "N/A".Localize(null, LocalizationScope.DefaultCategory) : value.ToString();
			}
			return retVal;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string parameterString = parameter as string;
			object retVal = null;

			if (parameterString == "Int2PropertyValueType")
				if (value is PropertyValueType)
					retVal = (int)(PropertyValueType)value;
				else
					retVal = String.Empty;

			return retVal;
		}
	}
}

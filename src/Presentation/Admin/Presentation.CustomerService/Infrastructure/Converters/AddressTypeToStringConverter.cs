using System;
using System.Windows.Data;

using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{
	public class AddressTypeToStringConverter:IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (string.IsNullOrEmpty(value.ToString()))
			{
				return null;
			}


			var addressType = value.ToString();
			AddressType result;
			if (Enum.TryParse(addressType, true, out result))
			{
				return result;
			}
			else
			{
				return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value;
		}

		#endregion
	}
}

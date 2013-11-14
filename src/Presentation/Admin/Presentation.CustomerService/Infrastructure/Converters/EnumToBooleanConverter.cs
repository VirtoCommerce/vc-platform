using System;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{
	public class EnumToBooleanConverter : IValueConverter
	{

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value.Equals(parameter);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value.Equals(true) ? parameter : Binding.DoNothing;
		}

		#endregion
	}
}

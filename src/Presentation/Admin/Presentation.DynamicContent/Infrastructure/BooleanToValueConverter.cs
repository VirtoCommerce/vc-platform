using System;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.DynamicContent.Infrastructure
{
	public class BooleanToValueConverter : IValueConverter
	{
		private const string ActiveValue = "Active";
		private const string NotActiveValue = "Not active";

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (bool)value ? ActiveValue : NotActiveValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}

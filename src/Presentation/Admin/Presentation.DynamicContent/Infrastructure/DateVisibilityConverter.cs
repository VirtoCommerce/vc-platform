using System;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.DynamicContent.Infrastructure
{
	public class DateVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (DateTime?)value == null ? "N/A" : value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}

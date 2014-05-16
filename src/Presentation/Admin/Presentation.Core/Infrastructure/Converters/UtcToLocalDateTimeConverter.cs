using System;
using System.Globalization;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Converters
{
	public class UtcToLocalDateTimeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DateTime.SpecifyKind(DateTime.Parse(value.ToString()), DateTimeKind.Utc).ToLocalTime();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

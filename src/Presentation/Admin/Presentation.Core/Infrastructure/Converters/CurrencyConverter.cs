using System;
using System.Windows.Data;
using System.Globalization;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Converters
{
	public class CurrencyConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var retVal = string.Empty;
			var inputDouble = value as double?;
			if (inputDouble.HasValue)
			{
				retVal = inputDouble.Value.ToString("c", culture);
			}
			else
			{
				var inputDecimal = value as decimal?;
				if (inputDecimal.HasValue)
				{
					retVal = inputDecimal.Value.ToString("c", culture);
				}
			}
			return retVal;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}

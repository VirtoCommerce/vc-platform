using System;
using System.Threading;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Order.Infrastructure.Converters
{
	public class DateToTextConverter : IValueConverter
	{
		private static ThreadLocal<DateToTextConverter> _instance = new ThreadLocal<DateToTextConverter>(() => new DateToTextConverter());
		public static DateToTextConverter Current
		{
			get
			{
				return _instance.Value;
			}
		}

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value != null ? value.ToString() : "Always effective".Localize();
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

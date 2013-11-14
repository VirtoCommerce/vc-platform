using System;
using System.Threading;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Fulfillment.Infrastructure
{
	public class ModifiedConverter : IValueConverter
	{
		private static ThreadLocal<ModifiedConverter> _instance = new ThreadLocal<ModifiedConverter>(() => new ModifiedConverter());
		public static ModifiedConverter Current
		{
			get
			{
				return _instance.Value;
			}
		}

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{			
			return value != null ? value : "-";
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

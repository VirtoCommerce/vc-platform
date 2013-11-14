using System;
using System.Threading;
using System.Windows.Data;
using VirtoCommerce.ManagementClient.Reviews.Model;

namespace VirtoCommerce.ManagementClient.Reviews.Infrastructure
{
	public class RatingConverter : IValueConverter
	{
		private static ThreadLocal<RatingConverter> _instance = new ThreadLocal<RatingConverter>(() => new RatingConverter());
		public static RatingConverter Current
		{
			get
			{
				return _instance.Value;
			}
		}

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            return value is ReviewBase && ((ReviewBase)value).ReviewType == ReviewType.Review ? ((ReviewBase)value).OverallRating.ToString() : "-";
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

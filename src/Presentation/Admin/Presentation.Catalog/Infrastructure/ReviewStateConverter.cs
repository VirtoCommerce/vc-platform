using System;
using System.Windows.Data;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.Infrastructure
{
	public class ReviewStateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (ReviewState)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var item = (ReviewState)value;
			return ToInt(item);
		}

	    public static int ToInt(ReviewState value)
	    {
			return value.GetHashCode();
	    }
	}
}

using System;
using System.Windows.Data;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.Infrastructure
{
	public class AvailabilityRuleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (AvailabilityRule)value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var item = (AvailabilityRule)value;
			return item.GetHashCode();
		}
	}
}

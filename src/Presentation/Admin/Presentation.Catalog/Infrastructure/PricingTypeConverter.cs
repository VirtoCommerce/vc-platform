using System;
using System.Windows.Data;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.Infrastructure
{
	public class PricingTypeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			object result;

			if (value is PricelistAssignment)
			{
				result = "Price List Assignment";
			}
			else
			{
				result = "Price List";
			}

			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

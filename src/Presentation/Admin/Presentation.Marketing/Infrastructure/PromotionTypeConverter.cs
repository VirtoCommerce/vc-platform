using System;
using System.Windows.Data;
using VirtoCommerce.Foundation.Marketing.Model;

namespace VirtoCommerce.ManagementClient.Marketing.Infrastructure
{
	public class PromotionTypeConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			object result;
			const string cartPromotionText = "Cart";
			const string catalogPromotionText = "Catalog";

			if (value is CartPromotion)
			{
				result = cartPromotionText;
			}
			else
			{
				result = catalogPromotionText;
			}

			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

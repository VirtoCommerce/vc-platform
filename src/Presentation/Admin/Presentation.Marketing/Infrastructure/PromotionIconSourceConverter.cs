using System;
using System.Threading;
using System.Windows.Data;
using VirtoCommerce.Foundation.Marketing.Model;

namespace VirtoCommerce.ManagementClient.Marketing.Infrastructure
{
	public class PromotionIconSourceConverter : IValueConverter
	{
		private static ThreadLocal<PromotionIconSourceConverter> _instance = new ThreadLocal<PromotionIconSourceConverter>(() => CreateInstance());
		public static PromotionIconSourceConverter Current
		{
			get
			{
				return _instance.Value;
			}
		}

		private static PromotionIconSourceConverter CreateInstance()
		{
			return new PromotionIconSourceConverter();
		}

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			string result;
			if (value is CartPromotion)
				result = "Icon_Promotion_Cart";
			else
				result = "Icon_Promotion_Catalog";
			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

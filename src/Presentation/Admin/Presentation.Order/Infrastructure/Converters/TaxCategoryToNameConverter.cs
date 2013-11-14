using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Data;
using VirtoCommerce.Foundation.Catalogs.Model;
using System.Linq;

namespace VirtoCommerce.ManagementClient.Order.Infrastructure.Converters
{
	public class TaxCategoryToNameConverter : IMultiValueConverter
	{
		private static readonly ThreadLocal<TaxCategoryToNameConverter> _instance =
			new ThreadLocal<TaxCategoryToNameConverter>(() => new TaxCategoryToNameConverter());

		public static TaxCategoryToNameConverter Current
		{
			get
			{
				return _instance.Value;
			}
		}

		public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var retVal = string.Empty;
			if (values != null)
			{

				var categoryId = values[0].ToString();

				var taxCategories = values[1] as List<TaxCategory> ;

				if (taxCategories == null)
					return string.Empty;

				var category = taxCategories.SingleOrDefault(c => c.TaxCategoryId == categoryId.ToString());
				if (category != null)
					retVal = category.Name;
			}
			return retVal;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}



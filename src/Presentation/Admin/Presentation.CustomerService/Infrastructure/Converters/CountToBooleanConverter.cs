using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Threading;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{
	public class CountToBooleanConverter : IValueConverter
	{

		private static readonly ThreadLocal<CountToBooleanConverter> _instance = new ThreadLocal<CountToBooleanConverter>(() => CreateInstance());
		public static CountToBooleanConverter Current
		{
			get
			{
				return _instance.Value;
			}
		}

		private static CountToBooleanConverter CreateInstance()
		{
			return new CountToBooleanConverter();
		}
		
		#region IvalueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var result = false;
			var items = value as IEnumerable<object>;

			if (items != null && items.Any())
			{
				result = true;
			}


			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

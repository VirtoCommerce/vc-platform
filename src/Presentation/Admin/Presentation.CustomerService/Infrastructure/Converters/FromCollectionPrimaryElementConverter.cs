using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{

	/// <summary>
	/// Convert IEnumerable<T> to first item of this enumerable/>
	/// if IEnumerable is null or Count==0, then return null;
	/// </summary>
	public class FromCollectionPrimaryElementConverter : IValueConverter
	{

		#region IValueConvertersMembers

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			object result = null;
			if (value != null)
			{
				var inputEnumerable = value as IEnumerable<object>;
				if (inputEnumerable != null && inputEnumerable.Any())
				{
					result = inputEnumerable.ElementAt(0);
				}
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

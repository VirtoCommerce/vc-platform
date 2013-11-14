using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{
	public class FromAddressCollectionPrimaryConverter : IValueConverter
	{

		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Address result = null;

			if (value != null)
			{
				var addrColl = value as ICollection<Address>;
				if (addrColl != null && addrColl.Count > 0)
				{
					result = addrColl.FirstOrDefault(a => a.Type.ToLower() == "Primary".ToLower());
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{
	public class FromEmailCollectionPrimaryConverter : IValueConverter
	{
		#region IvalueConverter

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Email result = null;

			if (value != null)
			{
				var emailColl = value as ICollection<Email>;
				if (emailColl != null && emailColl.Count > 0)
				{
					result = emailColl.FirstOrDefault(e => e.Type.ToLower() == "Primary".ToLower());
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

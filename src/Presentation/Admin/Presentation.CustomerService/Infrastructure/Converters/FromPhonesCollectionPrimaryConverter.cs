using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{
	public class FromPhonesCollectionPrimaryConverter : IValueConverter
	{

		#region IvalueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			Phone result = null;

			if (value != null)
			{
				var phoneColl = value as ICollection<Phone>;
				if (phoneColl != null && phoneColl.Count > 0)
				{
					result = phoneColl.FirstOrDefault(p => p.Type.ToLower() == "Primary".ToLower());
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

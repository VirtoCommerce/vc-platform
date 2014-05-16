using System;
using System.Windows.Data;

using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{
	public class CurrentUserNameConverter : IValueConverter
	{

		#region IvalueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
			{
				return string.Empty;
			}

			var userId = parameter.ToString();

			var contact = value as Contact;
			if (contact != null)
			{
				if (contact.MemberId == userId)
				{
					return "Me".Localize();
				}
			}

			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}

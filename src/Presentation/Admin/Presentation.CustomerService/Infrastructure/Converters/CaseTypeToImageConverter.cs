using System;
using System.Windows.Data;

using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{
	public class CaseTypeToImageConverter : IValueConverter
	{


		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}

			CaseChannel channel;

			Uri result = null;

			if (!Enum.TryParse(value.ToString(), out channel))
			{

			}
			else
			{
			    switch (channel)
			    {
			        case CaseChannel.Email:
			            result = new Uri(@"/VirtoCommerce.ManagementClient.Customers;component/Resources/images/mail.png",
			                             UriKind.Relative);
			            break;
			        case CaseChannel.Phone:
			            result = new Uri(
			                @"/VirtoCommerce.ManagementClient.Customers;component/Resources/images/inbound_call.png",
			                UriKind.Relative);
			            break;
			        case CaseChannel.CommerceManager:
			            break;
			    }
			}


			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}


	}
}

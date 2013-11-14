using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.ManagementClient.Fulfillment.Infrastructure
{
	public class SetIsEnabledConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			bool retVal = false;

            if (value != null && value is string)
            {
				retVal = value.ToString() == Enum.GetName(typeof(RmaLineItemState), RmaLineItemState.AwaitingReturn);
            }

			return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

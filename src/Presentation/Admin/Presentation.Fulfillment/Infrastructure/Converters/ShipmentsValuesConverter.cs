using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.ManagementClient.Fulfillment.Infrastructure
{
	public class ShipmentsValuesConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			var retVal = string.Empty;

            if (value != null)
            {
                var settingsCollection = value as ObservableCollection<Shipment>;
				retVal = string.Join(Environment.NewLine, settingsCollection.ToList().Select(val => val.ShipmentId));
            }

			return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

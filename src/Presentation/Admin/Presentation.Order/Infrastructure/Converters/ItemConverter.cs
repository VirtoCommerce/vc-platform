using System;
using System.Windows.Data;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.ManagementClient.Order.Infrastructure.Converters
{
    public class ItemConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string retVal = null;
            if (value is RmaReturnItem)
            {
                var rmaReturnItem = value as RmaReturnItem;
                value = rmaReturnItem.RmaLineItems[0].LineItem;
            }
            
            if (value is LineItem)
            {
                retVal = string.Format("{0}", ((LineItem)value).DisplayName);
            }
            else if (value is Item)
            {
                retVal = string.Format("{0}", ((Item)value).Name);
            }

            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

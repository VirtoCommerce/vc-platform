using System;
using System.Windows.Data;
using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.ManagementClient.Order.Infrastructure.Converters
{
    public class OrderAddressConverter : IValueConverter, IMultiValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string retVal = null;
            var memberAddress = value as OrderAddress;
            if (memberAddress != null)
            {
                retVal = string.Format("{2} {3} {1}, {0}", memberAddress.CountryName, memberAddress.City, memberAddress.Line1, memberAddress.Line2);
            }
            else if (value != null)
            {
                retVal = value.ToString();
            }
            return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        #endregion

        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(values[0], targetType, parameter, culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

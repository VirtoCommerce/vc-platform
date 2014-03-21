using System;
using System.Threading;
using System.Windows.Data;
using VirtoCommerce.Foundation.Orders.Model.Taxes;

namespace VirtoCommerce.ManagementClient.Order.Infrastructure.Converters
{
    public class TaxValueToJurisdictionGroupNameConverter : IMultiValueConverter
    {
        private static readonly ThreadLocal<TaxValueToJurisdictionGroupNameConverter> _instance =
            new ThreadLocal<TaxValueToJurisdictionGroupNameConverter>(() => new TaxValueToJurisdictionGroupNameConverter());

        public static TaxValueToJurisdictionGroupNameConverter Current
        {
            get
            {
                return _instance.Value;
            }
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var retVal = string.Empty;
            var item = values[0] as TaxValue;
            if (item != null && item.JurisdictionGroup != null)
                retVal = item.JurisdictionGroup.DisplayName;

            return retVal;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}



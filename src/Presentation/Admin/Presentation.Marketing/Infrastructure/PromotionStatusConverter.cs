using System;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Marketing.Infrastructure
{
    public class PromotionStatusConverter : IValueConverter
    {
        internal const string ValueActive = "Active";
        internal const string ValueInActive = "Inactive";

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Convert.ToString(value) == ValueActive;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Convert.ToBoolean(value) ? ValueActive : ValueInActive;
        }
    }
}

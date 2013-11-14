using System;
using System.Windows;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{
    public class ThicknessMaxConverter:IValueConverter 
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var thickness = (Thickness)value;
            var horizontalMax = Math.Max(thickness.Left, thickness.Right);
            var verticalMax = Math.Max(thickness.Top, thickness.Bottom);
            return Math.Max(horizontalMax, verticalMax);
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}

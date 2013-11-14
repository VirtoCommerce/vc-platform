using System;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Converters
{
    /// <summary>
    /// casts any object to string
    /// </summary>
    public class ObjectToStringConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

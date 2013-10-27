using System;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Converters
{
    /// <summary>
    /// generic enum to int value converter
    /// </summary>
    /// <typeparam name="T">enum</typeparam>
    public class EnumToIntConverter<T> : IValueConverter
    {
        public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (T)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return System.Convert.ToInt32((T)value);
        }
    }
}

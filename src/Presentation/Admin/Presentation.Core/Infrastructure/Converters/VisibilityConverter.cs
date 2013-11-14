using System;
using System.Windows;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Converters
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = Visibility.Collapsed;
            if (parameter == null) result = Visibility.Visible;
            else
            {
                if (value is int)
                {
                    var parsedValue = (int)value;
                    int parsedParameter;
                    int.TryParse(parameter.ToString(), out parsedParameter);
                    if (parsedValue >= parsedParameter) result = Visibility.Visible;
                }
                else
                {
                    if (value.ToString() == parameter.ToString()) result = Visibility.Visible;
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

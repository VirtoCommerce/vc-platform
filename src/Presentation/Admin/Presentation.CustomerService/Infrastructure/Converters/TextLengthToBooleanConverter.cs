using System;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{
    public class TextLengthToBooleanConverter:IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool result = false;

            if (value != null)
            {
                if (value is string)
                {
                    string inputString = value.ToString();

                    if (inputString.Length == 0)
                    {
                        result = true;
                    }
                }
            }
            else
            {
                result = true;
            }


            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

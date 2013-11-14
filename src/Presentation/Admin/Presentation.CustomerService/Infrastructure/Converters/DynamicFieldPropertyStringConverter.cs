using System;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Converters
{
    public class DynamicFieldPropertyStringConverter:IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null || !(parameter is string))
            {
                return string.Empty;
            }


            var relativeFieldName = parameter.ToString();
            var fieldName = value.ToString();


            if (string.IsNullOrEmpty(relativeFieldName) || string.IsNullOrEmpty(fieldName))
                return string.Empty;


            var result = relativeFieldName + "." + fieldName;


            return result;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

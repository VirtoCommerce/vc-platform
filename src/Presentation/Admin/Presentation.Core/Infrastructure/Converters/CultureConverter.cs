using System;
using System.Globalization;
using System.Windows.Data;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Converters
{
    public class CultureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object result;
            var valueString = value as string;
            if (string.IsNullOrEmpty(valueString))
                result = value;
            else
                if (valueString.Length > 6 && !valueString.Contains("-"))
					result = LocalizingConverter.Current.Convert(valueString, targetType, parameter, culture);
                else
                {
                    var valueCulture = CultureInfo.GetCultureInfo(valueString);
                    result = valueCulture.DisplayName;
                }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

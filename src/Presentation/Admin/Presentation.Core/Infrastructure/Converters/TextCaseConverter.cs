using System;
using System.Windows.Data;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Converters
{
    public class TextCaseConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = string.Empty;

            if (value != null)
                result = value.ToString().ToUpper();

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

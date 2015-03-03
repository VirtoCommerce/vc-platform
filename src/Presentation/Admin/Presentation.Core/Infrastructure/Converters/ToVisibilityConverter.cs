using System;
using System.Linq;
using System.Windows.Data;
using System.Threading;
using System.Windows;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Converters
{
    /// <summary>
    /// Конвертирует :
    /// - not-null значения и boolean true в Visibility.Visible;
    /// - null значения и boolean false в Visibility.Сollapsed;
    /// - Visibility значения в bool (Visibility.Visible -> true, для остальных - false)
    /// Если параметр (string) содержит 'i', то логика инвертируется (например boolean false будет конвертироваться в Visibility.Visible).
    /// Если параметр содержит 'h', то boolean false и nulls конвертируются в Visibility.Hidden.
    /// </summary>
    public class ToVisibilityConverter : IValueConverter
    {
        private static readonly ThreadLocal<ToVisibilityConverter> _instance = new ThreadLocal<ToVisibilityConverter>(() => CreateInstance());
        public static ToVisibilityConverter Current
        {
            get
            {
                return _instance.Value;
            }
        }

        private static ToVisibilityConverter CreateInstance()
        {
            return new ToVisibilityConverter();
        }

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool visible;
            if (value == null)
            {
                visible = false;
            }
            else if (value is bool)
            {
                visible = (bool)value;
            }
            else if (value is int)
            {
                visible = (int)value != 0;
            }
            else if (value is Visibility)
            {
                visible = (Visibility)value == Visibility.Visible;
            }
            else
            {
                visible = true;
            }

            var parameterStr = parameter as string;
            if (parameterStr != null)
            {
                if (parameterStr.Contains('i'))
                {
                    visible = !visible;
                }
                if (!visible && parameterStr.Contains('h'))
                {
                    return Visibility.Hidden;
                }
            }

            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

using Microsoft.Practices.ServiceLocation;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using VirtoCommerce.Foundation.Frameworks.Currencies;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Converters
{
    public class CurrencyConverter : IMultiValueConverter
    {
        private static readonly ICurrencyService _currencyService;

        static CurrencyConverter()
        {
            _currencyService = ServiceLocator.Current.GetInstance<ICurrencyService>();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal amount = 0;
            var isParsedDecimal = false;
            var inputDecimal = value as decimal?;
            if (inputDecimal.HasValue)
            {
                amount = inputDecimal.Value;
                isParsedDecimal = true;
            }
            else if (value != DependencyProperty.UnsetValue)
            {
                amount = (decimal)value;
                isParsedDecimal = true;
            }

            return isParsedDecimal ? _currencyService.FormatCurrency(amount, parameter as string) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values">[0] is money amount; [1] is currency code</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>formated money with currency</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(values[0], targetType, values[1], culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Windows.Data;
using VirtoCommerce.Foundation.Marketing.Model;
using System.Threading;

namespace VirtoCommerce.ManagementClient.Marketing.Infrastructure
{
    /// <summary>
    /// convert Promotion Status to 'State' to be displayed in Title.
    /// </summary>
    public class PromotionStateConverter : IValueConverter
    {
        private static ThreadLocal<PromotionStateConverter> _instance = new ThreadLocal<PromotionStateConverter>(() => new PromotionStateConverter());
        public static PromotionStateConverter Current
        {
            get
            {
                return _instance.Value;
            }
        }
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string result = null;
            if (value is Promotion)
            {
                var item = (Promotion)value;

                if ((PromotionStatusConverter.ValueActive == item.Status
                        && (item.StartDate > DateTime.UtcNow
                            || item.EndDate.HasValue && item.EndDate.Value < DateTime.UtcNow))
                    || string.IsNullOrEmpty(item.Status))
                    result = PromotionStatusConverter.ValueInActive;
                else
                    result = item.Status;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

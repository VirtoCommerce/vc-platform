using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using VirtoCommerce.ManagementClient.Core.Infrastructure.CronExpressionDescriptor;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.Infrastructure.Converters
{
	public class SystemJobScheduleCronToStringConverter : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			string retVal = string.Empty;

            if (value != null)
            {
				var options = new Options();
				options.DayOfWeekStartIndexZero = false;
				var ed = new ExpressionDescriptor(value.ToString(), options);
				retVal = ed.GetDescription(DescriptionTypeEnum.FULL);//.GetDescription(value.ToString());
            }

			return retVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

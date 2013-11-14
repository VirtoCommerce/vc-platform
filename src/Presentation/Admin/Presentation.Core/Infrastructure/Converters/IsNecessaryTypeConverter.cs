using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Reflection;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Converters
{
    public class IsNecessaryTypeConverter:IValueConverter
    {

        public Type NecessaryType { get; set; }

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (NecessaryType == null)
                return false;


           // Type t = Type.GetType(NecessaryType.FullName, false, true);

            

            bool result = false;

            if (value != null && value.GetType()==NecessaryType)
            {
                result = true;
            }

            return result;


        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion

        
    }
}

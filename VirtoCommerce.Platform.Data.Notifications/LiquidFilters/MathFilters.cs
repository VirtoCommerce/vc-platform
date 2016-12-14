using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Data.Notifications.LiquidFilters
{
    public class MathFilters
    {
        public static object Round(object input, int digits = 0)
        {
            if (input != null)
            {
                input = Math.Round(Convert.ToDouble(input, CultureInfo.InvariantCulture), digits);
            }
            return input;
        }

        public static object Ceil(object input)
        {
            if (input != null)
            {
                input = Math.Ceiling(Convert.ToDouble(input, CultureInfo.InvariantCulture));
            }
            return input;
        }

        public static object Floor(object input)
        {
            if (input != null)
            {
                input = Math.Floor(Convert.ToDouble(input, CultureInfo.InvariantCulture));
            }
            return input;
        }

        public static object Abs(object input)
        {
            if (input != null)
            {
                input = Math.Abs(Convert.ToDouble(input, CultureInfo.InvariantCulture));
            }
            return input;
        }

    }

}

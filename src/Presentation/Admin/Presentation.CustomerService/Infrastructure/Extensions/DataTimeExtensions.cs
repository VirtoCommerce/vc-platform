using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }
    }

}

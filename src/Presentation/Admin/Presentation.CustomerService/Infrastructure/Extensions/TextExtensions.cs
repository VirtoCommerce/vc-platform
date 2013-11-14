using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.ManagementClient.Customers.Infrastructure.Extensions
{
    public static class TextExtensions
    {

        public static int IndexOf<TSource>(this IEnumerable<TSource> source,
                                           Func<TSource, bool> predicate)
        {
            int i = 0;

            foreach (TSource element in source)
            {
                if (predicate(element))
                    return i;

                i++;
            }

            return -1;
        }

    }
}

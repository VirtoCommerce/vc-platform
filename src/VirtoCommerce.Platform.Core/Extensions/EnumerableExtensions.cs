using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class EnumerableExtensions
    {
        /// <summary>Indicates whether the specified enumerable is null or has a length of zero.</summary>
        /// <param name="data">The data to test.</param>
        /// <returns>true if the array parameter is null or has a length of zero; otherwise, false.</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> data)
        {
            return data == null || !data.Any();
        }

        public static int GetOrderIndependentHashCode<T>(this IEnumerable<T> source)
        {
            int hash = 0;
            //Need to force order to get  order independent hash code
            foreach (T element in source.OrderBy(x => x, Comparer<T>.Default))
            {
                hash = hash ^ EqualityComparer<T>.Default.GetHashCode(element);
            }
            return hash;
        }
    }
}

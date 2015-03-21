using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Web.Client.Extensions.Filters
{
    using VirtoCommerce.Foundation.Catalogs.Search;
    using VirtoCommerce.Foundation.Search;
    using VirtoCommerce.Foundation.Search.Schemas;

    public static class ISearchFilterExtension
    {
        public static ISearchFilterValue[] GetValues(this ISearchFilter filter)
        {
            var attributeFilter = filter as AttributeFilter;
            if (attributeFilter != null)
            {
                return attributeFilter.Values;
            }

            var rangeFilter = filter as RangeFilter;
            if (rangeFilter != null)
            {
                return rangeFilter.Values;
            }

            var priceRangeFilter = filter as PriceRangeFilter;
            if (priceRangeFilter != null)
            {
                return priceRangeFilter.Values;
            }

            var categoryFilter = filter as CategoryFilter;
            if (categoryFilter != null)
            {
                return categoryFilter.Values;
            }

            return null;
        }
    }
}

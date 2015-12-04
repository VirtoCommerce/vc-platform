using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Catalog.Model
{
    public class SearchAggregationRangeItem<T> : SearchAgregationItem<T>
    {
        public SearchAggregationRangeItem(T item)
            :base(item)
        {
        }
        public decimal? From { get; set; }
        public decimal? To { get; set; }
    }
}

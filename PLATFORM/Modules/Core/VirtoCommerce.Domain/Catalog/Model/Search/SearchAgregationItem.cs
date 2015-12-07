using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Domain.Catalog.Model
{
    public class SearchAgregationItem<T>
    {
        public SearchAgregationItem(T item)
        {
            Item = item;
        }
        public T Item { get; set; }
        public long Count { get; set; }
    }
}

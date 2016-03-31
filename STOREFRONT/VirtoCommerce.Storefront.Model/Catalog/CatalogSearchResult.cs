using System.Collections.Generic;
using PagedList;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class CatalogSearchResult
    {
        public IPagedList<Product> Products { get; set; }
        public Aggregation[] Aggregations { get; set; }
    }
}

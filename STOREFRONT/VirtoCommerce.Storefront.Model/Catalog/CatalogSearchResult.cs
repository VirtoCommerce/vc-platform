using System.Collections.Generic;
using PagedList;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class CatalogSearchResult
    {
        public IMutablePagedList<Product> Products { get; set; }
        public Category Category { get; set; }
        public IMutablePagedList<Category> Categories { get; set; }
        public IMutablePagedList<Aggregation> Aggregations { get; set; }
    }
}

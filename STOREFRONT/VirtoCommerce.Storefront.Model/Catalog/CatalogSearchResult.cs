using System.Collections.Generic;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class CatalogSearchResult
    {
        public IStorefrontPagedList<Product> Products { get; set; }
        public int TotalItemCount { get; set; }
        public Category Category { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public Aggregation[] Aggregations { get; set; }
    }
}

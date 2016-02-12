using System.Collections.Generic;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class CatalogSearchResult
    {
        public virtual IStorefrontPagedList<Product> Products { get; set; }

        public Category Category { get; set; }

        public IEnumerable<Category> Categories { get; set; }

        public virtual Aggregation[] Aggregations { get; set; }
    }
}

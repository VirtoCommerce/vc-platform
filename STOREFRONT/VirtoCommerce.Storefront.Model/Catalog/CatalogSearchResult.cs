using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public class CatalogSearchResult
    {
        public IStorefrontPagedList<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }

    }
}

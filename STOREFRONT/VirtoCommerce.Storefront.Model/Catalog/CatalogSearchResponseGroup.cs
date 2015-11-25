using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    [Flags]
    public enum CatalogSearchResponseGroup
    {
        WithProducts = 1,
        WithCategories = 2,
        WithProperties = 4,
        WithCatalogs = 8,
        WithVariations = 16,
        Full = WithProducts | WithCategories | WithProperties | WithCatalogs | WithVariations
    }
}

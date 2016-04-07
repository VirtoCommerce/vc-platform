using System;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    [Flags]
    public enum CatalogSearchResponseGroup
    {
        None = 0,
        WithProducts = 1,
        WithCategories = 2,
        WithProperties = 4,
        WithCatalogs = 8,
        WithVariations = 16,
        WithPriceRanges = 32,
        WithOutlines = 64,
        Full = WithProducts | WithCategories | WithProperties | WithCatalogs | WithVariations | WithPriceRanges | WithOutlines
    }
}

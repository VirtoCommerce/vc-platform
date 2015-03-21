using System;

namespace VirtoCommerce.CatalogModule.Model
{
    [Flags]
    public enum ResponseGroup
    {
        WithItems = 1,
        WithCategories = 2,
        WithProperties = 4,
        WithCatalogs = 8
    }
}

using System;

namespace VirtoCommerce.Domain.Catalog.Model
{
    [Flags]
    public enum ResponseGroup
    {
        WithProducts = 1,
        WithCategories = 2,
        WithProperties = 4,
        WithCatalogs = 8,
		WithVariations = 16
    }
}

using System;

namespace VirtoCommerce.Domain.Catalog.Model
{
    [Flags]
    public enum CategoryResponseGroup
    {
        None = 0,
        Info = 1 << 0,
        WithImages = 1 << 1,
        WithProperties = 1 << 2,
        WithLinks = 1 << 3,
        WithSeo = 1 << 4,
        WithParents = 1 << 5,
        WithOutlines = 1 << 6,
        Full = Info | WithImages | WithProperties | WithLinks | WithSeo | WithParents | WithOutlines
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    [Flags]
    public enum CategoryResponseGroup
    {
        Info = 0,
        WithImages = 1 << 1,
        WithProperties = 1 << 2,
        WithLinks = 1 << 3,
        WithSeo = 1 << 4,
        WithParents = 1 << 5,
        Full = Info | WithImages | WithProperties | WithLinks | WithSeo | WithParents
    }
}

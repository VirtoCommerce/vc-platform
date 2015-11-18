using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    public enum ItemResponseGroup
    {
        ItemInfo = 0,
        ItemWithPrices = 1 << 1,
        ItemWithInventories = 1 << 2,
        ItemWithMarketing = 1 << 3,
        ItemLarge = ItemInfo | ItemWithPrices | ItemWithInventories | ItemWithMarketing
    }
}

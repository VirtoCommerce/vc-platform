﻿using System;

namespace VirtoCommerce.Storefront.Model.Catalog
{
    [Flags]
    public enum ItemResponseGroup
    {
        ItemInfo = 0,
        ItemWithPrices = 1 << 1,
        ItemWithInventories = 1 << 2,
        ItemWithMarketing = 1 << 3,
        ItemLarge = ItemInfo | ItemWithPrices | ItemWithInventories | ItemWithMarketing
    }
}

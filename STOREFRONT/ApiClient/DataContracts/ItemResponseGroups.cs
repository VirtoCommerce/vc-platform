#region

using System;

#endregion

namespace VirtoCommerce.ApiClient.DataContracts
{

    #region

    #endregion

    [Flags]
    public enum ItemResponseGroups
    {
        ItemInfo = 0,

        ItemAssets = 1 << 1,

        ItemProperties = 1 << 2,

        ItemAssociations = 1 << 3,

        ItemEditorialReviews = 1 << 4,

        Variations = 1 << 5,

        Seo = 1 << 6,

        ItemSmall = ItemInfo | ItemAssets | ItemProperties | Seo,

        ItemMedium = ItemSmall | ItemAssociations | ItemEditorialReviews,

        ItemLarge = ItemMedium | Variations
    }
}

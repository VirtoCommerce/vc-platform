using System;

namespace VirtoCommerce.ApiClient.DataContracts
{
    [Flags]
    public enum ItemResponseGroups
    {
        ItemInfo = 0,
        ItemAssets = 1 << 1,
        ItemProperties = 1 << 2,
        ItemCategories = 1 << 3,
        ItemAssociations = 1 << 4,
        ItemEditorialReviews = 1 << 5,
        ItemCustomerReviews = 1 << 6,
        ItemSmall = ItemInfo | ItemAssets | ItemProperties,
        ItemMedium = ItemInfo | ItemAssets | ItemProperties | ItemAssociations | ItemEditorialReviews,
        ItemLarge = ItemInfo | ItemAssets | ItemProperties | ItemAssociations | ItemEditorialReviews | ItemCategories | ItemCustomerReviews
    }
}

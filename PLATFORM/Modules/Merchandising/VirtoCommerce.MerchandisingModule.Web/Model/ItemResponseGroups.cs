using System;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    [Flags]
    public enum ItemResponseGroups
    {
        /// <summary>
        /// Populated only base fields
        /// </summary>
        ItemInfo = 0,
        /// <summary>
        /// Populated assets collection
        /// </summary>
        ItemAssets = 1 << 1,
        /// <summary>
        /// Populated properties collection
        /// </summary>
        ItemProperties = 1 << 2,
        /// <summary>
        /// Populated categories collection
        /// </summary>
        ItemCategories = 1 << 3,
        /// <summary>
        /// Populated associations collection
        /// </summary>
        ItemAssociations = 1 << 4,
        /// <summary>
        /// Populated editorial reviews collection
        /// </summary>
        ItemEditorialReviews = 1 << 5,
        /// <summary>
        /// Populated base fields, assets and properties collections
        /// </summary>
        ItemSmall = ItemInfo | ItemAssets | ItemProperties,
        /// <summary>
        /// Populated base fields, assets, properties, associations and editorial reviews collections
        /// </summary>
        ItemMedium = ItemInfo | ItemAssets | ItemProperties | ItemAssociations | ItemEditorialReviews,
        /// <summary>
        /// Populated base fields, assets, properties, associations, editorial reviews and categories collections
        /// </summary>
        ItemLarge = ItemInfo | ItemAssets | ItemProperties | ItemAssociations | ItemEditorialReviews | ItemCategories
    }
}
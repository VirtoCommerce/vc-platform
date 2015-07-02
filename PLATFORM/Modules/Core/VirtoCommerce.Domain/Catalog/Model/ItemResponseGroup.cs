using System;

namespace VirtoCommerce.Domain.Catalog.Model
{
	[Flags]
	public enum ItemResponseGroup
	{
		ItemInfo = 0,
		ItemAssets = 1 << 1,
		ItemProperties = 1 << 2,
		ItemAssociations = 1 << 3,
		ItemEditorialReviews = 1 << 4,
		Variations = 1 << 5,
		Seo = 1 << 6,
        Categories = 1 << 7,
        Inventory = 1 << 8,
		ItemSmall = ItemInfo | ItemAssets | ItemProperties,
        ItemMedium = ItemSmall | ItemAssociations | ItemEditorialReviews,
        ItemLarge = ItemMedium | Variations | Seo | Categories | Inventory
	}
}

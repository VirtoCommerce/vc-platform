using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.Model
{
    public enum ItemSubtype
    {
        Product = 1,
        SKU = 2,
        Bundle = 3,
        Package = 4,
        Dynamickit = 5
    }

    public static class ItemSubtypeHelper
    {
        public static ItemSubtype GetItemType(Item item)
        {
            ItemSubtype result = ItemSubtype.Product;

            if (item != null)
            {
                var t = item.GetType();
                if (t == typeof(Sku)) result = ItemSubtype.SKU;
                else if (t == typeof(Bundle)) result = ItemSubtype.Bundle;
                else if (t == typeof(Package)) result = ItemSubtype.Package;
                else if (t == typeof(DynamicKit)) result = ItemSubtype.Dynamickit;
            }

            return result;
        }

        public static int GetItemTypeInt(Item item)
        {
            return (int)GetItemType(item);
        }
    }
}

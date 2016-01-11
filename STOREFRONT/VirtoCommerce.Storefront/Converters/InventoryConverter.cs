using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model.Catalog;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class InventoryConverter
    {
        public static Inventory ToWebModel(this VirtoCommerce.Client.Model.VirtoCommerceInventoryModuleWebModelInventoryInfo inventoryInfo)
        {
            var retVal = new Inventory();

            retVal.InjectFrom<NullableAndEnumValueInjecter>(inventoryInfo);
            retVal.Status = EnumUtility.SafeParse(inventoryInfo.Status, InventoryStatus.Disabled);

            return retVal;
        }
    }
}
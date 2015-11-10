using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Storefront.Model;
using Omu.ValueInjecter;

namespace VirtoCommerce.Storefront.Converters
{
    public static class InventoryConverter
    {
        public static Inventory ToWebModel(this VirtoCommerce.Client.Model.VirtoCommerceInventoryModuleWebModelInventoryInfo inventoryInfo)
        {
            var retVal = new Inventory();
            retVal.InjectFrom(inventoryInfo);
            return retVal;
        }
    }
}
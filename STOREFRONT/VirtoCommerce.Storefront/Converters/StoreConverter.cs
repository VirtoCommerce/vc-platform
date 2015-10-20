using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Models;

namespace VirtoCommerce.Storefront.Converters
{
    public static class StoreConverter
    {
        public static Store ToWebModel(this VirtoCommerce.Client.Model.VirtoCommerceStoreModuleWebModelStore storeDto)
        {
            var retVal = new Store();
            retVal.InjectFrom(storeDto);
            return retVal;
        }

    }
}
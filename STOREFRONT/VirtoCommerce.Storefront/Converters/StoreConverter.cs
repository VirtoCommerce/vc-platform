using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model;

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

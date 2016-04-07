using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Objects;
using VirtoCommerce.Storefront.Model.Common;
using StorefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class VendorConverter
    {
        public static Vendor ToShopifyModel(this StorefrontModel.Vendor storefrontModel)
        {
            var shopifyModel = new Vendor();

            shopifyModel.InjectFrom<NullableAndEnumValueInjecter>(storefrontModel);

            var shopifyAddressModels = storefrontModel.Addresses.Select(a => a.ToShopifyModel());
            shopifyModel.Addresses = new MutablePagedList<Address>(shopifyAddressModels);
            shopifyModel.DynamicProperties = storefrontModel.DynamicProperties;

            return shopifyModel;
        }
    }
}
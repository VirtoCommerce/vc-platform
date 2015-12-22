using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Objects;
using StorefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class LanguageConverter
    {
        public static Language ToShopifyModel(this StorefrontModel.Language storefrontModel)
        {
            var shopifyModel = new Language();

            shopifyModel.InjectFrom<StorefrontModel.Common.NullableAndEnumValueInjecter>(storefrontModel);
        
            return shopifyModel;
        }
    }
}
using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Objects;
using StorefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class PageConverter
    {
        public static Page ToShopifyModel(this StorefrontModel.StaticContent.ContentPage contentPage)
        {
            var retVal = new Page();
            retVal.InjectFrom<StorefrontModel.Common.NullableAndEnumValueInjecter>(contentPage);
            return retVal;
        }
    }
}
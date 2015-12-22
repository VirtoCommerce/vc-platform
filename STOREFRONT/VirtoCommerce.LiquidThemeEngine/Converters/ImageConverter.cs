using Omu.ValueInjecter;
using VirtoCommerce.LiquidThemeEngine.Objects;
using StorefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class ImageConverter
    {
        public static Image ToShopifyModel(this StorefrontModel.Image image)
        {
            var shopifyModel = new Image();

            shopifyModel.InjectFrom<StorefrontModel.Common.NullableAndEnumValueInjecter>(image);

            shopifyModel.Name = image.Title;
            shopifyModel.Src = image.Url;
        
            return shopifyModel;
        }
    }
}
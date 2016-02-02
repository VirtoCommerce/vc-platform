using Omu.ValueInjecter;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.LiquidThemeEngine.Objects;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class AttachmentConverter
    {
        public static Attachment ToShopifyModel(this VirtoCommerce.Storefront.Model.Attachment storefrontModel)
        {
            var shopifyModel = new Attachment();

            shopifyModel.InjectFrom<NullableAndEnumValueInjecter>(storefrontModel);

            return shopifyModel;
        }
    }
}
using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Converters.Injections;
using VirtoCommerce.LiquidThemeEngine.Objects;
using storefrontModel = VirtoCommerce.Storefront.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class ImageConverter
    {
        public static Image ToShopifyModel(this storefrontModel.Image image)
        {
            var result = new Image();
            result.InjectFrom<NullableAndEnumValueInjection>(image);
            result.Name = image.Title;
            result.Src = image.Url;
        
            return result;
        }

    }
}


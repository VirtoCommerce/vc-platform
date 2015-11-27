using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Converters.Injections;
using VirtoCommerce.LiquidThemeEngine.Objects;
using storefrontModel = VirtoCommerce.Storefront.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class LanguageConverter
    {
        public static Language ToShopifyModel(this storefrontModel.Language language)
        {
            var result = new Language();
            result.InjectFrom<NullableAndEnumValueInjection>(language);
        
            return result;
        }

    }
}


using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.LiquidThemeEngine.Converters.Injections;
using VirtoCommerce.LiquidThemeEngine.Objects;
using storefrontModel = VirtoCommerce.Storefront.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class CurrencyConverter
    {
        public static Currency ToShopifyModel(this storefrontModel.Common.Currency currency)
        {
            var result = new Currency();
            result.InjectFrom<NullableAndEnumValueInjection>(currency);

            return result;
        }

    }
}


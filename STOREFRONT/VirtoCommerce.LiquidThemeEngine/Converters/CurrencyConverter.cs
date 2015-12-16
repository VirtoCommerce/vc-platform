using Omu.ValueInjecter;
using VirtoCommerce.LiquidThemeEngine.Objects;
using StorefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class CurrencyConverter
    {
        public static Currency ToShopifyModel(this StorefrontModel.Common.Currency currency)
        {
            var result = new Currency();

            result.InjectFrom<StorefrontModel.Common.NullableAndEnumValueInjecter>(currency);

            return result;
        }
    }
}
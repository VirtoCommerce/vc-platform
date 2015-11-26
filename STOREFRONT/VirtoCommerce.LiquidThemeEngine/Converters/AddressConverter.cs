using Omu.ValueInjecter;
using VirtoCommerce.LiquidThemeEngine.Converters.Injections;
using VirtoCommerce.LiquidThemeEngine.Objects;
using storefrontModel = VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.Converters
{
    public static class AddressConverter
    {
        public static Address ToShopifyModel(this storefrontModel.Address address)
        {
            Address result = null;

            if (address != null)
            {
                result = new Address();
                result.InjectFrom<NullableAndEnumValueInjection>(address);
            }

            return result;
        }
    }
}

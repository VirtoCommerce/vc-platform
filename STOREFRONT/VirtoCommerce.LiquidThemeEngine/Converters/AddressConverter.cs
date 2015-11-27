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

                result.Address1 = address.Line1;
                result.Address2 = address.Line2;
                result.Street = string.Join(", ", result.Address1, result.Address2).Trim(',', ' ');
                result.Company = address.Organization;
                result.Province = address.RegionName;
                result.ProvinceCode = address.RegionId;
                result.Zip = address.PostalCode;
                result.Country = address.CountryName;
            }

            return result;
        }
    }
}

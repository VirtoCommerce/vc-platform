using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Pricing;

namespace VirtoCommerce.Storefront.Converters
{
    public static class PricelistConverter
    {
        public static Pricelist ToWebModel(this VirtoCommercePricingModuleWebModelPricelist pricelist)
        {
            var result = new Pricelist
            {
                Id = pricelist.Id,
                Currency = pricelist.Currency,
            };

            return result;
        }
    }
}

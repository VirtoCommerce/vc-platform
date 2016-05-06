using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class TierPriceConverter
    {
        public static TierPrice ToTierPrice(this VirtoCommercePricingModuleWebModelPrice serviceModel, Currency currency)
        {
            var listPrice = new Money(serviceModel.List ?? 0, currency);

            return new TierPrice(currency)
            {
                Quantity = serviceModel.MinQuantity ?? 1,
                Price = serviceModel.Sale.HasValue ? new Money(serviceModel.Sale.Value, currency) : listPrice
            };
        }

        public static TierPrice ToWebModel(this VirtoCommerceQuoteModuleWebModelTierPrice serviceModel, Currency currency)
        {
            var webModel = new TierPrice(currency);

            webModel.InjectFrom<NullableAndEnumValueInjecter>(serviceModel);

            webModel.Price = new Money(serviceModel.Price ?? 0, currency);

            return webModel;
        }

        public static VirtoCommerceQuoteModuleWebModelTierPrice ToQuoteServiceModel(this TierPrice webModel)
        {
            var serviceModel = new VirtoCommerceQuoteModuleWebModelTierPrice();

            serviceModel.InjectFrom<NullableAndEnumValueInjecter>(webModel);

            serviceModel.Price = (double)webModel.Price.Amount;

            return serviceModel;
        }
    }
}

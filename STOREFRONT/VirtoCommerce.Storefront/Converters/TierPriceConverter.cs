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

            return new TierPrice
            {
                ListPrice = listPrice,
                Quantity = serviceModel.MinQuantity ?? 1,
                SalePrice = serviceModel.Sale.HasValue ? new Money(serviceModel.Sale.Value, currency) : listPrice
            };
        }

        public static TierPrice ToWebModel(this VirtoCommerceQuoteModuleWebModelTierPrice serviceModel, Currency currency)
        {
            var webModel = new TierPrice();

            webModel.InjectFrom<NullableAndEnumValueInjecter>(serviceModel);

            webModel.ListPrice = new Money(serviceModel.Price ?? 0, currency);

            return webModel;
        }

        public static VirtoCommerceQuoteModuleWebModelTierPrice ToQuoteServiceModel(this TierPrice webModel)
        {
            var serviceModel = new VirtoCommerceQuoteModuleWebModelTierPrice();

            serviceModel.InjectFrom<NullableAndEnumValueInjecter>(webModel);

            serviceModel.Price = (double)webModel.ListPrice.Amount;

            return serviceModel;
        }
    }
}

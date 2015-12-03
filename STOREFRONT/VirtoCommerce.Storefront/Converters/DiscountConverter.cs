using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class DiscountConverter
    {
        public static Discount ToWebModel(this VirtoCommerceCartModuleWebModelDiscount serviceModel)
        {
            var webModel = new Discount();

            webModel.InjectFrom(serviceModel);

            webModel.Amount = new Money(serviceModel.DiscountAmount ?? 0, serviceModel.Currency);

            return webModel;
        }

        public static VirtoCommerceCartModuleWebModelDiscount ToServiceModel(this Discount webModel)
        {
            var serviceModel = new VirtoCommerceCartModuleWebModelDiscount();

            serviceModel.InjectFrom(webModel);

            serviceModel.Currency = webModel.Amount.CurrencyCode;
            serviceModel.DiscountAmount = (double)webModel.Amount.Amount;

            return serviceModel;
        }

        public static Discount ToWebModel(this VirtoCommerceOrderModuleWebModelDiscount serviceModel)
        {
            var webModel = new Discount();

            webModel.InjectFrom(serviceModel);

            webModel.Amount = new Money(serviceModel.DiscountAmount ?? 0, serviceModel.Currency);

            return webModel;
        }
    }
}
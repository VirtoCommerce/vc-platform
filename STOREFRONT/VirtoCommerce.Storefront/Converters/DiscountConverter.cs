using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Converters
{
    public static class DiscountConverter
    {
        public static Discount ToWebModel(this VirtoCommerceCartModuleWebModelDiscount serviceModel, IEnumerable<Currency> availCurrencies, Language language)
        {
            var webModel = new Discount();

            webModel.InjectFrom(serviceModel);
            var currency = availCurrencies.FirstOrDefault(x => x.Equals(serviceModel.Currency)) ?? new Currency(language, serviceModel.Currency);
            webModel.Amount = new Money(serviceModel.DiscountAmount ?? 0, currency);

            return webModel;
        }

        public static VirtoCommerceCartModuleWebModelDiscount ToServiceModel(this Discount webModel)
        {
            var serviceModel = new VirtoCommerceCartModuleWebModelDiscount();

            serviceModel.InjectFrom(webModel);

            serviceModel.Currency = webModel.Amount.Currency.Code;
            serviceModel.DiscountAmount = (double)webModel.Amount.Amount;

            return serviceModel;
        }

        public static Discount ToWebModel(this VirtoCommerceOrderModuleWebModelDiscount serviceModel, IEnumerable<Currency> availCurrencies, Language language)
        {
            var webModel = new Discount();

            webModel.InjectFrom(serviceModel);
            var currency = availCurrencies.FirstOrDefault(x => x.Equals(serviceModel.Currency)) ?? new Currency(language, serviceModel.Currency);
            webModel.Amount = new Money(serviceModel.DiscountAmount ?? 0, currency);

            return webModel;
        }
    }
}
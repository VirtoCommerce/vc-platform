using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Marketing;

namespace VirtoCommerce.Storefront.Converters
{
    public static class DiscountConverter
    {
        public static Discount ToWebModel(this VirtoCommerceCartModuleWebModelDiscount serviceModel, IEnumerable<Currency> availCurrencies)
        {
            var webModel = new Discount();

            webModel.InjectFrom(serviceModel);
            var currency = availCurrencies.FirstOrDefault(x => x.IsHasSameCode(serviceModel.Currency)) ?? new Currency(serviceModel.Currency, 1);
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

        public static Discount ToWebModel(this VirtoCommerceOrderModuleWebModelDiscount serviceModel, IEnumerable<Currency> availCurrencies)
        {
            var webModel = new Discount();

            webModel.InjectFrom(serviceModel);
            var currency = availCurrencies.FirstOrDefault(x => x.IsHasSameCode(serviceModel.Currency)) ?? new Currency(serviceModel.Currency, 1);
            webModel.Amount = new Money(serviceModel.DiscountAmount ?? 0, currency);

            return webModel;
        }
    }
}
using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Order;
using System.Collections.Generic;

namespace VirtoCommerce.Storefront.Converters
{
    public static class InPaymentConverter
    {
        public static PaymentIn ToWebModel(this VirtoCommerceOrderModuleWebModelPaymentIn paymentIn, IEnumerable<Currency> availCurrencies)
        {
            var webModel = new PaymentIn();

            var currency = availCurrencies.FirstOrDefault(x => x.IsHasSameCode(paymentIn.Currency)) ?? new Currency(paymentIn.Currency, 1); 

            webModel.InjectFrom(paymentIn);

            if (paymentIn.ChildrenOperations != null)
            {
                webModel.ChildrenOperations = paymentIn.ChildrenOperations.Select(co => co.ToWebModel(availCurrencies)).ToList();
            }

            webModel.Currency = currency;

            if (paymentIn.DynamicProperties != null)
            {
                webModel.DynamicProperties = paymentIn.DynamicProperties.Select(dp => dp.ToWebModel()).ToList();
            }

            webModel.Sum = new Money(paymentIn.Sum ?? 0, currency);
            webModel.Tax = new Money(paymentIn.Tax ?? 0, currency);

            return webModel;
        }
    }
}
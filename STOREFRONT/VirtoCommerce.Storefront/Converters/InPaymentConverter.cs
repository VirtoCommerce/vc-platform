using Omu.ValueInjecter;
using System.Linq;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Common;
using VirtoCommerce.Storefront.Model.Order;

namespace VirtoCommerce.Storefront.Converters
{
    public static class InPaymentConverter
    {
        public static PaymentIn ToWebModel(this VirtoCommerceOrderModuleWebModelPaymentIn paymentIn)
        {
            var webModel = new PaymentIn();

            var currency = new Currency(EnumUtility.SafeParse(paymentIn.Currency, CurrencyCodes.USD));

            webModel.InjectFrom(paymentIn);

            if (paymentIn.ChildrenOperations != null)
            {
                webModel.ChildrenOperations = paymentIn.ChildrenOperations.Select(co => co.ToWebModel()).ToList();
            }

            webModel.Currency = currency;

            if (paymentIn.DynamicProperties != null)
            {
                webModel.DynamicProperties = paymentIn.DynamicProperties.Select(dp => dp.ToWebModel()).ToList();
            }

            webModel.Sum = new Money(paymentIn.Sum ?? 0, currency.Code);
            webModel.Tax = new Money(paymentIn.Tax ?? 0, currency.Code);

            return webModel;
        }
    }
}
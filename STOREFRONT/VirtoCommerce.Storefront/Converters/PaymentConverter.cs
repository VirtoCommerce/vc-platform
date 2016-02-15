using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class PaymentConverter
    {
        public static Payment TowebModel(this VirtoCommerceCartModuleWebModelPayment payment, Currency currency)
        {
            var webModel = new Payment(currency);

            webModel.InjectFrom(payment);

            webModel.Amount = new Money(payment.Amount ?? 0, currency);

            if (payment.BillingAddress != null)
            {
                webModel.BillingAddress = payment.BillingAddress.ToWebModel();
            }

            webModel.Currency = currency;

            return webModel;
        }

        public static VirtoCommerceCartModuleWebModelPayment ToServiceModel(this Payment payment)
        {
            var serviceModel = new VirtoCommerceCartModuleWebModelPayment();

            serviceModel.InjectFrom(payment);

            serviceModel.Amount = (double)payment.Amount.Amount;

            if (payment.BillingAddress != null)
            {
                serviceModel.BillingAddress = payment.BillingAddress.ToCartServiceModel();
            }

            serviceModel.Currency = payment.Currency.Code;

            return serviceModel;
        }
    }
}
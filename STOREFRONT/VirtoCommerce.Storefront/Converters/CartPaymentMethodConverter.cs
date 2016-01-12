using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Cart;
using VirtoCommerce.Storefront.Model.Common;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CartPaymentMethodConverter
    {
        public static PaymentMethod ToWebModel(this VirtoCommerceCartModuleWebModelPaymentMethod paymentMethod)
        {
            var paymentMethodWebModel = new PaymentMethod();

            paymentMethodWebModel.InjectFrom(paymentMethod);

            return paymentMethodWebModel;
        }

        public static Payment ToPaymentModel(this PaymentMethod paymentMethod, Money amount, Currency currency)
        {
            var paymentWebModel = new Payment(currency);

            paymentWebModel.Amount = amount;
            paymentWebModel.Currency = currency;
            paymentWebModel.PaymentGatewayCode = paymentMethod.GatewayCode;

            return paymentWebModel;
        }
    }
}
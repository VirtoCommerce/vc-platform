using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model.Cart;

namespace VirtoCommerce.Storefront.Converters
{
    public static class PaymentConverter
    {
        public static Payment TowebModel(this VirtoCommerceCartModuleWebModelPayment payment)
        {
            var paymentWebModel = new Payment();

            paymentWebModel.InjectFrom(payment);
            if (payment.BillingAddress != null)
            {
                paymentWebModel.BillingAddress = payment.BillingAddress.ToWebModel();
            }

            return paymentWebModel;
        }
    }
}
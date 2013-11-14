using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Virto.Helpers.Payments
{
	/// <summary>
	/// Class for Credit card payment option
	/// </summary>
    public class CreditCardOption : IPaymentOption
    {
		/// <summary>
		/// Gets the key of the payment option which is used to identify specific option.
		/// </summary>
		/// <value>The key.</value>
        public string Key
        {
            get
            {
                return "CreditCard";
            }
        }

		/// <summary>
		/// Validates input data for the control. In case of Credit card pre authentication will be the best way to
		/// validate. The error message if any should be displayed within a control itself.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>Returns false if validation is failed.</returns>
        public bool ValidateData(PaymentModel model)
        {
            return true;
        }

        /// <summary>
        /// This method is called before the order is completed. This method should check all the parameters
        /// and validate the credit card or other parameters accepted.
        /// </summary>
        /// <param name="form">The form.</param>
        /// <param name="model">The model.</param>
        /// <returns>payment</returns>
        public Payment PreProcess(OrderForm form, PaymentModel model)
        {
            var cardPayment = new CreditCardPayment
                {
                    BillingAddressId = form.BillingAddressId,
                    CreditCardCustomerName = model.CustomerName,
                    CreditCardNumber = model.CardNumber,
                    CreditCardExpirationMonth = model.ExpirationMonth,
                    CreditCardExpirationYear = model.ExpirationYear,
                    CreditCardSecurityCode = model.CardVerificationNumber,
                    CreditCardType = model.CardType,
                    PaymentType = PaymentType.CreditCard.GetHashCode(),
                    PaymentTypeId = 0,
                    PaymentMethodName = model.DisplayName,
                    PaymentMethodId = model.Id,
					Amount = form.Total
                };

            return cardPayment;
        }

        /// <summary>
        /// This method is called after the order is placed. This method should be used by the gateways that want to
        /// redirect customer to their site.
        /// </summary>
        /// <param name="orderForm">The order form.</param>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c></returns>
        public bool PostProcess(OrderForm orderForm, PaymentModel model)
        {
            return true;
        }
    }
}
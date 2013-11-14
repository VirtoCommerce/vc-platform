using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Virto.Helpers.Payments
{
    public interface IPaymentOption
    {
        /// <summary>
        /// Gets the key of the payment option which is used to identify specific option.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        string Key { get; }

        /// <summary>
        /// Validates input data for the control. In case of Credit card pre authentication will be the best way to
        /// validate. The error message if any should be displayed within a control itself.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Returns false if validation is failed.
        /// </returns>
        bool ValidateData(PaymentModel model);

        /// <summary>
        /// This method is called before the order is completed. This method should check all the parameters
        /// and validate the credit card or other parameters accepted.
        /// </summary>
        /// <param name="orderForm">The order form.</param>
        /// <param name="model">The model.</param>
        /// <returns>
        /// Payment
        /// </returns>
        Payment PreProcess(OrderForm orderForm, PaymentModel model);

        /// <summary>
        /// This method is called after the order is placed. This method should be used by the gateways that want to
        /// redirect customer to their site.
        /// </summary>
        /// <param name="orderForm">The order form.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        bool PostProcess(OrderForm orderForm, PaymentModel model);
    }

}
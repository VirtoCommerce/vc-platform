using System.Collections.Generic;
using VirtoCommerce.Foundation.Orders.Model;
using VirtoCommerce.Foundation.Orders.Services;

namespace VirtoCommerce.PaymentGateways
{
    public abstract class PaymentGatewayBase : IPaymentGateway
    {
        #region IPaymentGateway Members

	    /// <summary>
	    /// Returns the configuration data associated with a gateway.
	    /// Sets the configuration gateway data. This data typically includes information like gateway URL, account info and so on.
	    /// </summary>
	    /// <value>The settings.</value>
	    /// <returns></returns>
	    public virtual IDictionary<string, string> Settings { get; set; }

	    /// <summary>
        /// Processes the payment. Can be used for both positive and negative transactions.
        /// </summary>
        /// <param name="payment">The payment.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public abstract bool ProcessPayment(Payment payment, ref string message);

        #endregion
    }
}

using VirtoCommerce.Foundation.Orders.Model;

namespace VirtoCommerce.PaymentGateways
{
	public class DefaultPaymentGateway : PaymentGatewayBase
	{
		/// <summary>
		/// Processes the payment. Can be used for both positive and negative transactions.
		/// </summary>
		/// <param name="payment">The payment.</param>
		/// <param name="message">The message.</param>
		/// <returns></returns>
		public override bool ProcessPayment(Payment payment, ref string message)
		{
			payment.Status = PaymentStatus.Processing.ToString();
			return true;
		}
	}
}

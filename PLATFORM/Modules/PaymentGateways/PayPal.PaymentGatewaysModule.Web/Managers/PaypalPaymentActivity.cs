using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Workflow;
using VirtoCommerce.Domain.Payment.Services;

namespace PayPal.PaymentGatewaysModule.Web.Managers
{
	public class PaypalPaymentActivity : IObserver<OrderStateBasedEvalContext>
	{
		private IPaymentGateway _paymentGateway;

		public PaypalPaymentActivity(IPaymentGateway paymentGateway)
		{
			_paymentGateway = paymentGateway;
		}

		public void OnCompleted()
		{
		}

		public void OnError(System.Exception error)
		{
		}

		public void OnNext(OrderStateBasedEvalContext value)
		{
			var payments = new List<PaymentIn>();

			if (value.ModifiedOrder != null)
			{
				payments = value.ModifiedOrder.InPayments.Where(p => !p.IsApproved && p.GatewayCode.Equals(_paymentGateway.GatewayCode)).ToList();
			}

			foreach (var payment in payments)
			{
				_paymentGateway.CreatePayment(payment, value.ModifiedOrder);
			}
		}
	}
}
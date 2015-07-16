using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Events;
using VirtoCommerce.Domain.Payment.Model;
using VirtoCommerce.Domain.Payment.Services;

namespace VirtoCommerce.OrderModule.Data.Observers
{
	public class CancelPaymentObserver : IObserver<OrderChangeEvent>
	{
		private readonly IPaymentMethodsService _paymentMethodService;

		public CancelPaymentObserver(IPaymentMethodsService paymentMethodService)
		{
			_paymentMethodService = paymentMethodService;
		}

		public void OnCompleted()
		{
			
		}

		public void OnError(Exception error)
		{
			
		}

		public void OnNext(OrderChangeEvent value)
		{
			CancelationPayment(value);
		}

		private void CancelationPayment(OrderChangeEvent value)
		{
			if (value.OrigOrder != null && value.OrigOrder.InPayments != null && value.OrigOrder.InPayments.Count > 0)
			{
				if (value.ModifiedOrder.InPayments != null && value.ModifiedOrder.InPayments.Count(p => p.PaymentStatus == PaymentStatus.Cancelled) > 0)
				{
					foreach (var payment in value.ModifiedOrder.InPayments.Where(p => p.PaymentStatus == PaymentStatus.Cancelled))
					{
						var op = value.OrigOrder.InPayments.FirstOrDefault(p => p.Id == payment.Id);
						if (op != null && op.PaymentStatus != PaymentStatus.Cancelled && op.PaymentStatus != PaymentStatus.Voided && op.PaymentStatus != PaymentStatus.Refunded)
						{
							var method = _paymentMethodService.GetAllPaymentMethods().FirstOrDefault(p => p.Code == op.GatewayCode);
							if(method != null)
							{
								if(op.PaymentStatus == PaymentStatus.Authorized)
								{
									method.VoidProcessPayment(new VoidProcessPaymentEvaluationContext { Payment = payment });
								}
								else if(op.PaymentStatus == PaymentStatus.Paid)
								{
									method.RefundProcessPayment(new RefundProcessPaymentEvaluationContext { Payment = payment });
								}
							}
						}
					}
				}
			}
		}
	}
}

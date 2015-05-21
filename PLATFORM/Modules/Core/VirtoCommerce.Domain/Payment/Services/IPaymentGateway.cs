using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Payment.Model;

namespace VirtoCommerce.Domain.Payment.Services
{
	public interface IPaymentGateway
	{
		string GatewayCode { get; }
		string Description { get; }
		string LogoUrl { get; }
		PaymentGatewayType GatewayType { get; }

		PaymentInfo GetPayment(string paymentId, string orderId);
		PaymentInfo CreatePayment(PaymentInfo paymentInfo);
	}
}

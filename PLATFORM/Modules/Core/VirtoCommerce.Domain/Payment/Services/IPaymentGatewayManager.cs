using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Payment.Services
{
	public interface IPaymentGatewayManager
	{
		void RegisterGateway(IPaymentGateway gateway);
		IEnumerable<IPaymentGateway> PaymentGateways { get; }
	}
}

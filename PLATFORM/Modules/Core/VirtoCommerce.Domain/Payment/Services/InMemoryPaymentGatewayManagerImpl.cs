using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Payment.Services
{
	public class InMemoryPaymentGatewayManagerImpl : IPaymentGatewayManager
	{
		private List<IPaymentGateway> _paymentGateways = new List<IPaymentGateway>();
		#region IPaymentGatewayManager Members

		public void RegisterGateway(IPaymentGateway gateway)
		{
			if(gateway == null)
			{
				throw new ArgumentNullException("gateway");
			}
			if(_paymentGateways.Any(x=>x.GatewayCode == gateway.GatewayCode))
			{
				throw new OperationCanceledException(gateway.GatewayCode + " already registered");
			}
			_paymentGateways.Add(gateway);
		}

		public IEnumerable<IPaymentGateway> PaymentGateways
		{
			get { return _paymentGateways.AsReadOnly(); }
		}

		#endregion
	}
}

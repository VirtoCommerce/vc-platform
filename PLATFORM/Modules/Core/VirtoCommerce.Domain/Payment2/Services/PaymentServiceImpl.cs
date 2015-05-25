using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Payment2.Model;

namespace VirtoCommerce.Domain.Payment2.Services
{
	public class PaymentServiceImpl : IPaymentService
	{
		private List<Func<PaymentMethod>> _paymentMethods = new List<Func<PaymentMethod>>();

		#region IPaymentService Members

		public Model.PaymentMethod[] GetAllPaymentMethods()
		{
			return _paymentMethods.Select(x => x()).ToArray();
		}

		public void RegisterPaymentMethod(Func<Model.PaymentMethod> methodGetter)
		{
			if (methodGetter == null)
			{
				throw new ArgumentNullException("methodGetter");
			}

			_paymentMethods.Add(methodGetter);
		}

		#endregion
	}
}

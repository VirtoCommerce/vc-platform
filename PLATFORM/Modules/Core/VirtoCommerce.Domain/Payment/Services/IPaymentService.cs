using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Payment.Model;

namespace VirtoCommerce.Domain.Payment.Services
{
	public interface IPaymentMethodsService
	{
		PaymentMethod[] GetAllPaymentMethods();
		void RegisterPaymentMethod(Func<PaymentMethod> methodGetter);
	}
}

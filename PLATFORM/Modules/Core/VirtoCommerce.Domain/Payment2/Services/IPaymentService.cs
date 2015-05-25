using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Payment2.Model;

namespace VirtoCommerce.Domain.Payment2.Services
{
	public interface IPaymentService
	{
		PaymentMethod[] GetAllPaymentMethods();
		void RegisterPaymentMethod(Func<PaymentMethod> methodGetter);
	}
}

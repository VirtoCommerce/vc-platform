using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Shipping.Model;

namespace VirtoCommerce.Domain.Shipping.Services
{
	public interface IShippingMethodsService
	{
		ShippingMethod[] GetAllShippingMethods();
		void RegisterShippingMethod(Func<ShippingMethod> methodFactory);
	}
}

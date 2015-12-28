using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Shipping.Model;

namespace VirtoCommerce.Domain.Shipping.Services
{
	public class ShippingMethodsServiceImpl : IShippingMethodsService
	{
		private List<Func<ShippingMethod>> _shippingMethods = new List<Func<ShippingMethod>>();
		
		#region IShippingService Members

		public Model.ShippingMethod[] GetAllShippingMethods()
		{
			return _shippingMethods.Select(x => x()).ToArray();
		}

		public void RegisterShippingMethod(Func<Model.ShippingMethod> methodFactory)
		{
			if (methodFactory == null)
			{
				throw new ArgumentNullException("methodFactory");
			}

			_shippingMethods.Add(methodFactory);
		}

		#endregion
	}
}

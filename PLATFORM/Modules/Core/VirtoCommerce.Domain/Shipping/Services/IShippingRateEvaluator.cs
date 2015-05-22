using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Shipping.Model;

namespace VirtoCommerce.Domain.Shipping.Services
{
	public interface IShippingRateEvaluator
	{
		IEnumerable<ShippingRate> EvaluateShippingRates(IEvaluationContext context);
	}
}

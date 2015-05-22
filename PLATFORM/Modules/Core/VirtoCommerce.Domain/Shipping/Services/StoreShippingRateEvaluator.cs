using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Shipping.Model;
using VirtoCommerce.Domain.Store.Services;

namespace VirtoCommerce.Domain.Shipping.Services
{
	public class StoreShippingRateEvaluator : IShippingRateEvaluator
	{
		private readonly IStoreService _storeService;
		public StoreShippingRateEvaluator(IStoreService storeService)
		{
			_storeService = storeService;
		}

		#region IShippingRateEvaluator Members

		public IEnumerable<Model.ShippingRate> EvaluateShippingRates(Common.IEvaluationContext context)
		{
			var shippingEvalContext = context as ShippingEvaluationContext;
			if(shippingEvalContext == null)
			{
				throw new NullReferenceException("shippingEvalContext");
			}

			var store = _storeService.GetById(shippingEvalContext.StoreId);
			if(store == null)
			{
				throw new NullReferenceException("store");
			}

			var retVal = store.ShippingMethods.Where(x => x.IsActive).Select(x => x.CalculateRate(context));
			return retVal;
		}

		#endregion
	}
}

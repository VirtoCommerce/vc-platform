using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Marketing.Repositories;

namespace VirtoCommerce.Foundation.Marketing.Model
{
	public class PromotionUsageProvider : IPromotionUsageProvider
	{
        private IMarketingRepository _repository;
		public PromotionUsageProvider(IMarketingRepository repository)
		{
			_repository = repository;
		}
		#region IPromotionUsageProvider Members

		public int GetTotalUsageCount(string promotionId)
		{
			var retVal = _repository.PromotionUsages.Count(x => x.PromotionId == promotionId);
			return retVal;
		}

		public int GetUsagePerCustomerCount(string promotionId, string customerId)
		{
			var retVal = _repository.PromotionUsages.Count(x => x.PromotionId == promotionId && x.MemberId == customerId);
			return retVal;
		}

		#endregion
	}
}
